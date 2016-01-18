// site.js
(function () {
    
    
    var $sidebarAndWrapper = $("#sidebar,#wrapper"); // wrapped set this AND that //$naming shows its a jQuery variable

    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $(this).text("Hide Sidebar");
        }
        else {
            $(this).text("Show Sidebar");
        }
    })
    





    //var ele = $("#username");
    //ele.text("Abraham Lincoln")

    //var main = $("#main");
    //main.on("mouseenter", function () {
    //    main.style = "background-color: #888;";
    //});
    //main.on("mouseleave", function () {
    //    main.style = "";
    //});

    //var menuItems = $("ul.menu li a");
    //menuItems.on("click", function()     
    //{
    //    var me = $(this);
    //    alert(me.text());
    //});
})();


