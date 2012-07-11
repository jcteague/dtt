Validator = require('validator').Validator

Validator::error = (msg) ->
    @_errors.push msg

Validator::get_errors = (msg) ->
    @_errors

module.exports = Validator
