var PageManagerApplication = new Backbone.Marionette.Application();

PageManagerApplication.addRegions({
    header: '#header',
    main: '#main',
    sidebar: '#sidebar'
});



PageManagerApplication.navigate = function (route, options) {
    options || (options = {});
    Backbone.history.navigate(route, options);
};

PageManagerApplication.on('initialize:after', function () {
    Backbone.history.start();
});