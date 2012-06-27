define 'client_router', ['backbone'], (Backbone) ->

    class ClientRouter extends Backbone.Router

        routes:
            ':p*any': 'render_path'
            '*action': 'render_root'

        render_root: ->
            @trigger 'render', '/'

        render_path: (routes...) ->
            @trigger 'render', routes.join('')

