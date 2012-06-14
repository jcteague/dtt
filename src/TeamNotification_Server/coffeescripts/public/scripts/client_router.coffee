define 'client_router', ['backbone'], (Backbone) ->

    class ClientRouter extends Backbone.Router

        routes:
            ':path': 'render_path'
            '*action': 'render_root'

        render_root: ->
            @trigger 'render', '/'

        render_path: (path) ->
            @trigger 'render', path

