// Full-document showcase only: no library interop or persistent theme preference.
const results = document.getElementById('contrast-results');

if (results) {
    const styles = getComputedStyle(document.documentElement);
    const context = document.createElement('canvas').getContext('2d', { willReadFrequently: true });
    const token = name => styles.getPropertyValue(`--graphite-${name}`).trim();
    const rgba = value => {
        context.clearRect(0, 0, 1, 1);
        context.fillStyle = value;
        context.fillRect(0, 0, 1, 1);
        return [...context.getImageData(0, 0, 1, 1).data];
    };
    const composite = (front, back) => front.slice(0, 3).map((value, index) =>
        value * front[3] / 255 + back[index] * (1 - front[3] / 255));
    const luminance = rgb => rgb.map(value => value / 255).map(value =>
        value <= .04045 ? value / 12.92 : ((value + .055) / 1.055) ** 2.4)
        .reduce((total, value, index) => total + value * [.2126, .7152, .0722][index], 0);
    const ratio = (front, back) => {
        const values = [luminance(front), luminance(back)].sort((a, b) => b - a);
        return (values[0] + .05) / (values[1] + .05);
    };
    // Sanity checks guard the measurement tool independently of this palette.
    console.assert(Math.abs(ratio([255, 255, 255], [0, 0, 0]) - 21) < .001, 'Contrast reference');
    console.assert(ratio([80, 80, 80], [80, 80, 80]) === 1, 'Equal colors');
    console.assert(composite([200, 100, 0, 0], [30, 40, 50])[0] === 30, 'Transparent overlay');

    const rows = [];
    const measure = (label, foreground, background, minimum = 4.5) => {
        const back = composite(rgba(token(background)), rgba(token('background')));
        const front = composite(rgba(token(foreground)), back);
        const contrast = ratio(front, back);
        const hex = color => `#${color.map(value => Math.round(value).toString(16).padStart(2, '0')).join('').toUpperCase()}`;
        const row = document.createElement('tr');
        row.dataset.pass = String(contrast >= minimum);
        row.dataset.ratio = String(contrast);
        row.dataset.minimum = String(minimum);
        for (const text of [label, `${hex(front)} / ${hex(back)}`, `${contrast.toFixed(2)}:1 ${contrast >= minimum ? 'Pass' : 'Below target'}`]) {
            const cell = document.createElement('td');
            cell.textContent = text;
            row.append(cell);
        }
        rows.push(row);
    };
    for (const surface of ['background', 'surface1', 'surface2', 'surface3']) {
        measure(`Body / ${surface}`, 'foreground', surface);
        measure(`Muted / ${surface}`, 'neutral-60', surface);
        measure(`Subtle / ${surface}`, 'neutral-50', surface);
        measure(`Control edge / ${surface}`, 'control-border', surface, 3);
        measure(`Focus / ${surface}`, 'focus', surface, 3);
    }
    for (const tone of ['accent', 'success', 'warning', 'danger', 'info']) {
        measure(`${tone} / default`, `${tone}-foreground`, tone);
        measure(`${tone} / hover`, `${tone}-foreground`, `${tone}-hover`);
        measure(`${tone} / pressed`, `${tone}-foreground`, `${tone}-active`);
        measure(`${tone} / subtle`, `${tone}-subtle-foreground`, `${tone}-subtle`);
        measure(`${tone} / focus on surface 3`, 'focus', 'surface3', 3);
    }
    results.replaceChildren(...rows);

    document.addEventListener('click', event => {
        const button = event.target.closest('[data-preview-action]');
        if (button) {
            document.getElementById('preview-feedback').textContent = `${button.dataset.previewAction} selected. Preview only; nothing saved.`;
        }
    });

    const sample = document.getElementById('theme-sample');
    const reportHeight = () => parent.postMessage({ type: 'graphite-preview-height', height: Math.ceil(sample.getBoundingClientRect().height) }, location.origin);
    new ResizeObserver(reportHeight).observe(sample);
} else {
    const frames = [...document.querySelectorAll('#theme-comparison iframe')];
    window.addEventListener('message', event => {
        if (event.origin !== location.origin || event.data?.type !== 'graphite-preview-height') return;
        const frame = frames.find(item => item.contentWindow === event.source);
        const height = event.data.height;
        if (frame && Number.isFinite(height) && height > 0 && height < 20000) {
            frame.height = String(height + 2);
            frame.style.height = `${height + 2}px`;
        }
    });
}
