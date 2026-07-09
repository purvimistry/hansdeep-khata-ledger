document.addEventListener("DOMContentLoaded", () => {
    const sidebar = document.querySelector(".sidebar");
    const menuToggle = document.getElementById("menuToggle");
    const sidebarClose = document.getElementById("sidebarClose");
    menuToggle?.addEventListener("click", (e) => {
        e.stopPropagation();
        sidebar?.classList.add("sidebar-open");
    });

    sidebarClose?.addEventListener("click", () => {
        sidebar?.classList.remove("sidebar-open");
    });
    document.querySelectorAll(".nav-item").forEach(item => {

        item.addEventListener("click", () => {

            if (window.innerWidth <= 992) {
                sidebar?.classList.remove("sidebar-open");
            }

            document.querySelectorAll(".nav-item")
                .forEach(nav => nav.classList.remove("active"));

            item.classList.add("active");
        });

    });


    const toast = document.getElementById("appToast");

    if (toast) {
        toast.classList.add("show");
        setTimeout(closeToast, 3500);
    }
});
function closeToast() {

    const toast = document.getElementById("appToast");

    if (!toast)
        return;

    toast.classList.add("hide");

    setTimeout(() => toast.remove(), 350);
}