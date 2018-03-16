describe("when extending two different types", function () {
    var firstTypeDefinition = Dolittle.Type.extend(function () { });
    var secondTypeDefinition = Dolittle.Type.extend(function () { });

    it("should have different type identifiers", function () {
        expect(firstTypeDefinition._typeId).not.toBe(secondTypeDefinition._typeId);
    });
});