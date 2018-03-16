describe("when getting all with one resolver registered", function () {

    var wellKnownTypesDependencyResolver = Dolittle.WellKnownTypesDependencyResolver;
    var defaultDependencyResolver = Dolittle.DefaultDependencyResolver;
    var knownArtifactTypesDependencyResolver = Dolittle.KnownArtifactTypesDependencyResolver;
    var knownArtifactInstancesDependencyResolver = Dolittle.KnownArtifactInstancesDependencyResolver;

    Dolittle.WellKnownTypesDependencyResolver = function () {
        this.isWellKnown = true;
    };

    Dolittle.DefaultDependencyResolver = function () {
        this.isDefault = true;
    };

    Dolittle.KnownArtifactTypesDependencyResolver = function () { };
    Dolittle.KnownArtifactInstancesDependencyResolver = function () { };

    Dolittle.dependencyResolvers.myResolver = {
        identifier: "Hello"
    };

    var resolvers = Dolittle.dependencyResolvers.getAll();

    Dolittle.WellKnownTypesDependencyResolver = wellKnownTypesDependencyResolver;
    Dolittle.DefaultDependencyResolver = defaultDependencyResolver;
    Dolittle.KnownArtifactTypesDependencyResolver = knownArtifactTypesDependencyResolver;
    Dolittle.KnownArtifactInstancesDependencyResolver = knownArtifactInstancesDependencyResolver;

    it("should not get any functions resolvers", function () {
        var hasFunction = false;

        for (var i = 0; i < resolvers.length; i++) {
            if (typeof resolvers[i] === "function") {
                hasFunction = true;
            }
        }

        expect(hasFunction).toBe(false);
    });

    it("should have the registered resolver at the end", function () {
        expect(resolvers[resolvers.length - 1].identifier).toBe("Hello");
    });

    it("should have the well known type resolver at the second place", function () {
        expect(resolvers[0].isWellKnown).toBe(true);
    });

    it("should have the default resolver at the second place", function () {
        expect(resolvers[1].isDefault).toBe(true);
    });
});