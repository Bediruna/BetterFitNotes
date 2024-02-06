function initializeScrollHandling(dotNetReference, elementId) {
    const element = document.getElementById(elementId);
    element.onscroll = () => {
        const scrollTop = element.scrollTop;
        const scrollHeight = element.scrollHeight;
        const offsetHeight = element.offsetHeight;
        const bottomReached = scrollTop + offsetHeight >= scrollHeight;
        const topReached = scrollTop === 0;

        if (bottomReached) {
            dotNetReference.invokeMethodAsync('LoadMoreMonths', false);
        } else if (topReached) {
            dotNetReference.invokeMethodAsync('LoadMoreMonths', true);
        }
    };
}

window.initializeScrollHandling = initializeScrollHandling;
