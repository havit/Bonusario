export function initialize(dotnetObjectReference, entryCardFormWrapperId) {
    let body = document.querySelector("body");

    if (body) {
        body.addEventListener("click", (event) => handleClick(event, dotnetObjectReference, entryCardFormWrapperId));
    }
}

function handleClick(event, dotnetObjectReference, entryCardFormWrapperId) {
    let entryCardFormWrapper = document.getElementById(entryCardFormWrapperId);

    if (!entryCardFormWrapper.contains(event.target)) {
        dotnetObjectReference.invokeMethodAsync("HandleBodyClick");
    }
}
