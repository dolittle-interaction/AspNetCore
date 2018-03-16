describe("when asking for extenders in root namespace", function () {
    var initialType = function (anInitialType) { };
    var namespace = null;
    var extenders = null;

    beforeEach(function () {
        Dolittle.dependencyResolver = {
            getDependenciesFor: sinon.stub()
        };
        initialType = Dolittle.Type.extend(initialType);
        Dolittle.namespace("Root", { RootExtendedType: initialType.extend(function () { }) });
        Dolittle.namespace("Root.Sub", { SubExtendedType: initialType.extend(function (foo) { }) });
        Dolittle.namespace("Root.AnotherSub", { ASecondExtendedType: initialType.extend(function (bar) { }) });

        namespace = Root.Sub.SubExtendedType._namespace;

        (function because_of() { extenders = initialType.getExtendersIn(namespace) })();;
    });

    afterEach(function () {
        Dolittle.functionParser = {};
        extenders = null;
    });

    it("should return extender on the sub level", function () {
        expect(extenders).toContain(Root.RootExtendedType);
    });
    
    it("should return extender on the root level", function () {
        expect(extenders).toContain(Root.RootExtendedType);
    });
    
    it("should only return 2 extenders", function () {
        expect(extenders.length).toBe(2);
    });
});
