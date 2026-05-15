document.addEventListener("DOMContentLoaded", () => {
    const toggle = document.querySelector(".password-toggle");
    const passwordInput = document.querySelector("#Password");

    if (!toggle || !passwordInput) return;

    const eyeOpen = toggle.querySelector(".eye-open");
    const eyeClosed = toggle.querySelector(".eye-closed");

    toggle.addEventListener("click", () => {
        const isPassword = passwordInput.getAttribute("type") === "password";

        passwordInput.setAttribute("type", isPassword ? "text" : "password");

        eyeOpen.classList.toggle("d-none", !isPassword);
        eyeClosed.classList.toggle("d-none", isPassword);
    });
});