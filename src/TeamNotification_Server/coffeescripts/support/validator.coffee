_ = require('underscore')
Validator = require('validator').Validator

Validator::error = (msg) ->
    @_errors.push msg
    @

Validator::get_errors = (msg) ->
    _.uniq @_errors

module.exports = Validator
