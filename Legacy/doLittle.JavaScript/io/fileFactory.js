Dolittle.namespace("Dolittle.io", {
    fileFactory: Dolittle.Singleton(function () {
        /// <summary>Represents a factory for creating instances of Dolittle.io.File</summary>
        this.create = function (path, fileType) {
            /// <summary>Creates a new file</summary>
            /// <param name="path" type="String">Path of file</param>
            /// <param name="fileType" type="Dolittle.io.fileType" optional="true">Type of file to use</param>
            /// <returns type="Dolittle.io.File">An instance of a file</returns>

            var file = Dolittle.io.File.create({ path: path });
            if (!Dolittle.isNullOrUndefined(fileType)) {
                file.fileType = fileType;
            }
            return file;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.fileFactory = Dolittle.io.fileFactory;