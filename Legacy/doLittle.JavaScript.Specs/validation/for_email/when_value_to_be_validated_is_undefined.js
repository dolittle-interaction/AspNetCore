describe("when value to be validated is undefined", function () {
    var validator = Dolittle.validation.email.create({ options: {} });
    var result = validator.validate(undefined);

    it("should be false", function () {
        expect(result).toBe(false); 
    });
});