Dolittle.namespace("Dolittle.Visualizer", {
    index: Dolittle.views.ViewModel.extend(function () {
        var self = this;

        this.categories = [
            { name: "QualityAssurance", displayName: "Quality Assurance", description: "" },
            { name: "Tasks", displayName: "Tasks", description: "" }
        ];

        this.currentCategory = ko.observable("Visualizer/"+this.categories[0].name+"/index");

        this.selectCategory = function (category) {
            self.currentCategory("Visualizer/" + category.name + "/index");
        };
    })
});

ko.bindingHandlers.sidebar = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        $("body").mouseover(function (e) {
            if ($(e.target).closest("table.DolittleSidebarIcons").length != 1) {
                $("#icons").removeClass("DolittleSidebarWithContent");
                $("#icons").removeClass("DolittleSidebarFullSize");
            }

        });

        $("#sidebar").mouseover(function () {
            $("#icons").addClass("DolittleSidebarIconsVisible");
        });

        $("#sidebar").mouseout(function (e) {
            $("#icons").removeClass("DolittleSidebarIconsVisible");

        });

        $("#icons").mouseover(function () {
            //$("#sidebar").addClass("DolittleSidebarFullSize");
        });

        $("#icons").mouseout(function (e) {
            $("#icons").addClass("DolittleSidebarFullSize");
        });


        $("#icons").click(function () {
            $("#icons").addClass("DolittleSidebarWithContent");
        });
    }
}
