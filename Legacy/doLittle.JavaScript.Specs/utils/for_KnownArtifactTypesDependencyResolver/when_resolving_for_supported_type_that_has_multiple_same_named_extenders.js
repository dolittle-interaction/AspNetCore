describe("when resolving for supported type that has multiple same named extenders", sinon.test(function () {

    var resolver,
        resolvedTypes,
        propertyToResolve,
        namespace,
        getExtendersIn = function (namespace) { return [Test.extender, Test.Deeper.extender, Test.Deeper.EvenDeeper.extender]; };

    beforeEach(function () {
        Dolittle.namespace("Test", { extender: Dolittle.Type.extend(function () { }) });
        Dolittle.namespace("Test.Deeper", { extender: Dolittle.Type.extend(function () { }) });
        Dolittle.namespace("Test.Deeper.EvenDeeper", { extender: Dolittle.Type.extend(function () { }) });
        namespace = Test.Deeper.EvenDeeper.extender._namespace;

        Dolittle.commands.Command = { getExtendersIn: getExtendersIn };

        resolver = new Dolittle.KnownArtifactTypesDependencyResolver();
        propertyToResolve = "commandTypes";
        namespace = {};
        
        resolvedTypes = resolver.resolve(namespace, propertyToResolve);
    });

    it("should resolve types", function () {
        expect(resolvedTypes).toBeDefined();
    })

    it("should have the most specific type", function () {
        expect(resolvedTypes.extender).toBe(Test.Deeper.EvenDeeper.extender);
    })

}));