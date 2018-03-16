Dolittle.namespace("Dolittle.Visualizer.QualityAssurance", {
    index: Dolittle.views.ViewModel.extend(function (allProblems) {
        var self = this;

        this.allProblems = allProblems.all().execute();


        this.getSeverityImageSrc = function (severity) {
            return "/Dolittle/Visualizer/QualityAssurance/warning.png";
        };
    })
});