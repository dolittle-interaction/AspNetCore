describe("when resolving a type that is registered", function () {
    Dolittle.WellKnownTypesDependencyResolver.types.something = "Hello";

    var resolver = new Dolittle.WellKnownTypesDependencyResolver();

    var result = resolver.resolve(null, "something");

    it("should return the system it resolves to", function () {
        expect(result).toBe("Hello");
    });
});