describe("when asking if string is object", function () {

    var result = Dolittle.isObject("something");

    it("should not be considered an object", function () {
        expect(result).toBe(false);
    });
    
});