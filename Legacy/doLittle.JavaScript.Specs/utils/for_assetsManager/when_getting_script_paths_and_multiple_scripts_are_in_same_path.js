describe("when getting script paths and multiple scripts are in same path", function () {
    var self = this;
    var paths = [];

    beforeEach(function () {
        Dolittle.namespaces = Dolittle.namespaces || {};
        Dolittle.namespaces.initialize = sinon.stub();
        sinon.stub($, "get", function (url, options, callback, type) {
            callback([
                "Something/cool.js",
                "Something/cooler.js",
                "Else/cool.js",
                "Else/cooler.js"
            ]);
        });

        Dolittle.path = {
            getPathWithoutFilename: function (path) {
                if (path.indexOf("Something") == 0) {
                    return "Something";
                }

                if (path.indexOf("Else") == 0) {
                    return "Else";
                }
            }
        }

        Dolittle.assetsManager.initialize();
        paths = Dolittle.assetsManager.getScriptPaths();
    });

    afterEach(function () {
        $.get.restore();
        Dolittle.assetsManager.scripts = [];
    });


    this.getCountFor = function (path) {
        var count = 0;

        for (var index = 0; index < paths.length; index++) {
            if (paths[index].indexOf(path) == 0) {
                count++;
            }
        }
        return count;
    };


    it("should return first path only once", function () {
        expect(self.getCountFor("Something")).toBe(1);
    });

    it("should return second path only once", function () {
        expect(self.getCountFor("Else")).toBe(1);
    });
});