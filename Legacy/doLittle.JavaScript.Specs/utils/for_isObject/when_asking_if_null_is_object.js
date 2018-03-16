describe("when asking if null is object", function () {

    var result = Dolittle.isObject(null);

    it("should not be considered an object", function () {
        expect(result).toBe(false);
    });
    
});