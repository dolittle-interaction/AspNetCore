describe("when extending", function () {
    var typeDefinition = function (something) { };
    var result = null;

    beforeEach(function () {
        Dolittle.dependencyResolver = {
            getDependenciesFor: sinon.stub()
        };
        result = Dolittle.Type.extend(typeDefinition);
    });

    afterEach(function () {
        Dolittle.functionParser = {};
    });

    it("should get the dependencies for the function", function () {
        expect(Dolittle.dependencyResolver.getDependenciesFor.called).toBe(true);
    });

    it("should return the type definition", function () {
        expect(result).toBe(typeDefinition);
    });

    it("should a create function", function () {
        expect(typeof result.create).toBe("function");
    });

    it("should add a type id", function () {
        expect(typeDefinition._typeId).toBeDefined();
    });

    it("should add this type to the list of types that extend Dolittle Type", function () {
        var extenders = Dolittle.Type.getExtenders();
        expect(extenders).toContain(typeDefinition);
    });
});