// PIN Input Helper Functions
window.focusElement = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.focus();
    }
};

window.selectText = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.select();
    }
};

// Prevent copy/paste in PIN inputs for security
window.addPinSecurity = () => {
    document.querySelectorAll('.pin-input').forEach(input => {
        input.addEventListener('paste', (e) => {
            e.preventDefault();
        });
        
        input.addEventListener('copy', (e) => {
            e.preventDefault();
        });
        
        input.addEventListener('cut', (e) => {
            e.preventDefault();
        });
        
        input.addEventListener('contextmenu', (e) => {
            e.preventDefault();
        });
    });
};

// Initialize PIN security when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.addPinSecurity();
});

// Re-apply security when new PIN inputs are added
const observer = new MutationObserver(() => {
    window.addPinSecurity();
});

observer.observe(document.body, {
    childList: true,
    subtree: true
}); 