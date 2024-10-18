// site.js


// Check local storage on page load
(function () {
    const mode = localStorage.getItem('mode');
    if (mode === 'light') {
        document.body.classList.add('light-mode');
    } else {
        document.body.classList.add('dark-mode'); 
    }
})();



function toggleMode() {
    const body = document.body;
    const modeLabel = document.getElementById('modeLabel');

    // Toggle the classes for dark and light mode
    if (body.classList.contains('dark-mode')) {
        body.classList.remove('dark-mode');
        body.classList.add('light-mode');
        modeLabel.textContent = "Dark"; // Change label
        localStorage.setItem('mode', 'light'); // Store light mode in local storage
    } else {
        body.classList.remove('light-mode');
        body.classList.add('dark-mode');
        modeLabel.textContent = "Light"; // Change label
        localStorage.setItem('mode', 'dark'); // Store dark mode in local storage
    }

    // Call a function to update footer text color if necessary
    updateFooterTextColor();
}

function updateFooterTextColor() {
    const footerText = document.getElementById('footer-text');

    // Change footer text color based on mode
    if (document.body.classList.contains('dark-mode')) {
        footerText.style.color = "#ffffff"; // Light color for footer text
    } else {
        footerText.style.color = "#000000"; // Dark color for footer text
    }
}

// Initial setup of footer text color based on loaded mode
updateFooterTextColor();
