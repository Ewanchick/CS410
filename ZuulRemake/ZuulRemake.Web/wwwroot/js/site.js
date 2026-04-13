function toggleMenu(event) {
    // This stops the click from hitting the window.onclick function
    event.stopPropagation();
    document.getElementById("myDropup").classList.toggle("show");
}

window.onclick = function (event) {
    // This now only runs if you click truly "outside"
    var dropups = document.getElementsByClassName("dropup-content");
    for (var i = 0; i < dropups.length; i++) {
        var openDropup = dropups[i];
        if (openDropup.classList.contains('show')) {
            openDropup.classList.remove('show');
        }
    }
}

