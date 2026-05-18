$(document).ready(function () {
    const sidebar = $('.sidebar');

    $('#menuToggle').on('click', function (e) {
        e.stopPropagation();
        sidebar.addClass('sidebar-open');
    });

    $('#sidebarClose').on('click', function () {
        sidebar.removeClass('sidebar-open');
    });

    $('.nav-item').on('click', function () {
        if ($(window).width() <= 992) {
            sidebar.removeClass('sidebar-open');
        }
        $('.nav-item').removeClass('active');
        $(this).addClass('active');
    });
});
