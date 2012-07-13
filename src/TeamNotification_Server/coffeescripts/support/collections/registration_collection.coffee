class RegistrationCollection

    to_json: ->
        self = '/registration'
        return {
            href: self
            links: [
                {rel: 'Registration', name: 'self', href: self}
            ]
            template:
                data: [
                    {name: 'first_name', label: 'First Name', type: 'string'}
                    {name: 'last_name', label: 'Last Name', type: 'string'}
                    {name: 'email', label: 'Email', type: 'string'}
                    {name: 'password', label: 'Password', type: 'password-confirmed'}
                ]
        }

module.exports = RegistrationCollection
