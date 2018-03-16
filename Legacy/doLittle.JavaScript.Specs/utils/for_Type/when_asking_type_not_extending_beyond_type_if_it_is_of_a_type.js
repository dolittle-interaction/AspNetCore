describe("when asking type not extending beyond type if it is of a type", function () {

    var a = Dolittle.Type.extend(function () { });
    var b = Dolittle.Type.extend(function () { });

    var result = b.typeOf(a);

    it("should return false", function () {
        expect(result).toBe(false);
    });
});