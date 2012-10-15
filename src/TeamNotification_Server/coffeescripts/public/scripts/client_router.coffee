define 'client_router', ['backbone'], (Backbone) ->

    class ClientRouter extends Backbone.Router

        routes:
            ':p*any': 'render_path'
            '*action': 'render_root'

        render_root: ->
            console.log 'rendering the root'
            #@trigger 'render', '/'
            #@trigger 'render', 'http://api.dtt.local:3000/?callback=?'
            #@trigger 'render', 'http://api.dtt.local:3000/?callback=?'
            @trigger 'render', '/api'

        render_path: (routes...) ->
            console.log 'rendering the path'
            @trigger 'render', "/api/#{routes.join('')}"

