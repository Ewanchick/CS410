// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const clickArea = document.getElementById('click-area');
const contextMenu = document.getElementById('context-menu');

// 1. Show menu on left-click
clickArea.addEventListener('click', (e) => {
    // Prevent default behavior if needed
    e.preventDefault();

    // Position the menu at the mouse cursor
    contextMenu.style.top = `${e.pageY}px`;
    contextMenu.style.left = `${e.pageX}px`;
    contextMenu.style.display = 'block';

    // Stop propagation so the 'window' click doesn't immediately hide it
    e.stopPropagation();
});

// 2. Hide menu when clicking anywhere else
window.addEventListener('click', () => {
    contextMenu.style.display = 'none';
});
