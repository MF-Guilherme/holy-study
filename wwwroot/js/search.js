document.getElementById("search").addEventListener("input", function () {
    const query = this.value.toLowerCase();
    const themes = document.querySelectorAll("#theme-list .card");

    themes.forEach((theme) => {
        const title = theme.querySelector(".card-title").textContent.toLowerCase();
        if (title.includes(query)) {
            theme.parentElement.style.display = "block"; // Mostra o card
        } else {
            theme.parentElement.style.display = "none"; // Esconde o card
        }
    });
});
