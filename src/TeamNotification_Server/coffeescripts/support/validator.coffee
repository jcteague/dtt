_ = require('underscore')
Validator = require('validator').Validator

Validator::error = (msg) ->
    @_errors.push msg
    @

Validator::get_errors = (msg) ->
    _.uniq @_errors

Validator::is_valid = ->
    @_errors.length isnt 0

module.exports = Validator
