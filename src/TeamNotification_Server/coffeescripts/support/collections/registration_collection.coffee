_ = require('underscore')

class RegistrationCollection

    constructor: (@data) ->

    to_json: ->
        self = '/registration'
        get_or_empty = (property) =>
            if @data? and @data[property]? then @data[property] else ''

        return {
            href: self
            links: [
                {rel: 'Registration', name: 'self', href: self}
            ]
            template:
                type: 'registration'
                data: [
                    {name: 'first_name', label: 'First Name', type: 'string', value: get_or_empty('first_name'), rules: {required: true}}
                    {name: 'last_name', label: 'Last Name', type: 'string', value: get_or_empty('last_name'), rules: {required: true}}
                    {name: 'email', label: 'Email', type: 'string', value: get_or_empty('email'), rules: {required: true, email: true}}
                    {name: 'password', label: 'Password', type: 'password', value: get_or_empty('password'), rules: {required: true, minlength: 6}}
                    {name: 'confirm_password', label: 'Confirm Password', type: 'password', value: get_or_empty('confirm_password'), rules: {required: true, minlength: 6, equalTo: 'password'}}
                ]
        }

    fill: (data) ->
        new RegistrationCollection(_.extend({}, @data, data))



module.exports = RegistrationCollection
