export function createObservedRootLifecycle({ selector, setup, destroy, isInitialized }) {
    let domObserver = null;
    const trackedRoots = new Set();

    function collectRootsFromNode(node) {
        if (!(node instanceof Element)) {
            return [];
        }

        const roots = [];

        if (node.matches(selector)) {
            roots.push(node);
        }

        node.querySelectorAll(selector).forEach((root) => roots.push(root));

        return roots;
    }

    function maybeTrackRoot(root) {
        if (!root) {
            return;
        }

        if (isInitialized(root)) {
            trackedRoots.add(root);
            return;
        }

        trackedRoots.delete(root);
    }

    function setupAndTrack(root) {
        if (!root) {
            return;
        }

        setup(root);
        maybeTrackRoot(root);
    }

    function destroyAndUntrack(root) {
        if (!root) {
            return;
        }

        trackedRoots.delete(root);
        destroy(root);
    }

    function cleanupDisconnectedRoots() {
        for (const root of Array.from(trackedRoots)) {
            if (!root.isConnected) {
                destroyAndUntrack(root);
            }
        }
    }

    function handleDomMutations(records) {
        for (const record of records) {
            for (const addedNode of record.addedNodes) {
                for (const root of collectRootsFromNode(addedNode)) {
                    setupAndTrack(root);
                }
            }

            for (const removedNode of record.removedNodes) {
                for (const root of collectRootsFromNode(removedNode)) {
                    destroyAndUntrack(root);
                }
            }
        }

        cleanupDisconnectedRoots();
    }

    function ensureObserverStarted() {
        if (domObserver || !document.body) {
            return;
        }

        domObserver = new MutationObserver(handleDomMutations);
        domObserver.observe(document.body, {
            childList: true,
            subtree: true
        });
    }

    function refresh(root = document) {
        if (!root) {
            return;
        }

        ensureObserverStarted();

        if (root instanceof Element && root.matches(selector)) {
            setupAndTrack(root);
        }

        if (typeof root.querySelectorAll === 'function') {
            root.querySelectorAll(selector).forEach(setupAndTrack);
        }

        cleanupDisconnectedRoots();
    }

    return {
        refresh,
        destroy: destroyAndUntrack
    };
}
