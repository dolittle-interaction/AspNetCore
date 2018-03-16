describe("when creating asynchronously with defined parameters", function () {

    Dolittle.dependencyResolver = {
        beginResolve: function (namespace, name) {
            var promise = Dolittle.execution.Promise.create();
            promise.signal(name);
            return promise;
        },
        getDependenciesFor: function () {
            return ["options"];
        }
    }

    var type = Dolittle.Type.extend(function (options) {
        this.options = options;
    });


    var result = null;
    type.beginCreate({
        options: {
            value: "Hello"
        }
    }).continueWith(function (parameter, nextPromise) {
        result = parameter;
    });

    it("should pass along the options", function () {
        expect(result.options.value).toBe("Hello");
    });
});