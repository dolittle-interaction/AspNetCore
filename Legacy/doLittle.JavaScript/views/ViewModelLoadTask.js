Dolittle.namespace("Dolittle.views", {
    ViewModelLoadTask: Dolittle.views.ComposeTask.extend(function (files, fileManager) {
        /// <summary>Represents a task for loading viewModels</summary>
        var self = this;

        this.files = [];
        files.forEach(function (file) {
            self.files.push(file.path.fullPath);
        });

        this.execute = function () {
            var promise = Dolittle.execution.Promise.create();

            fileManager.load(files).continueWith(function (instances) {
                promise.signal(instances);
            });
            return promise;
        };
    })
});