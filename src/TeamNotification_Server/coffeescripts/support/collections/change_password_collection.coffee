_ = require('underscore')

class ChangePasswordCollection

    constructor: (@data) ->

    to_json: ->
        self = "/reset_password/#{@data.reset_key}"
        get_or_empty = (property) =>
            if @data? and @data[property]? then @data[property] else ''

        return {
            href: self
            links: [
                {rel: 'Change password', name: 'self', href: self}
                {"name": "Login", "rel": "login", 'href':'/user/login'}
            ]
            template:
                type: 'change_password'
                data: [
                    {name: 'password', label: 'New password', type: 'password', value: get_or_empty('password'), rules: {required: true, minlength: 6}}
                    {name: 'confirm_password', label: 'Confirm password', type: 'password', value: get_or_empty('confirm_password'), rules: {required: true, minlength: 6, equalTo: 'password'}}
                ]
        }

    fill: (data) ->
        new ChangePasswordCollection(_.extend({}, @data, data))



module.exports = ChangePasswordCollection
