function initializeScrollHandling(dotNetReference, elementId) {
    const element = document.getElementById(elementId);
    const tolerance = 1; // Define a tolerance value, adjust as needed

    element.onscroll = () => {
        const scrollTop = element.scrollTop;
        const scrollHeight = element.scrollHeight;
        const offsetHeight = element.offsetHeight;

        // Adjust the bottomReached condition to include the tolerance
        const bottomReached = (scrollTop + offsetHeight + tolerance) >= scrollHeight;
        const topReached = scrollTop === 0;

        if (bottomReached) {
            dotNetReference.invokeMethodAsync('LoadMoreMonths', false);
        } else if (topReached) {
            dotNetReference.invokeMethodAsync('LoadMoreMonths', true);
        }
    };
}

window.initializeScrollHandling = initializeScrollHandling;