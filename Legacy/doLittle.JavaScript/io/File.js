Dolittle.namespace("Dolittle.io", {
    File: Dolittle.Type.extend(function (path) {
        /// <summary>Represents a file</summary>

        /// <field name="type" type="Dolittle.io.fileType">Type of file represented</field>
        this.type = Dolittle.io.fileType.unknown;

        /// <field name="path" type="Dolittle.Path">The path representing the file</field>
        this.path = Dolittle.Path.create({ fullPath: path });
    })
});