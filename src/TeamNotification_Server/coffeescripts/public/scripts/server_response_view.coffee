define 'server_response_view', ['backbone'], (Backbone) ->

    class ServerResponseView extends Backbone.View

        id: 'server-response-container'

        render: ->
            @$el.empty()
            @$el.append "<p>#{message}</p>" for message in @model
            @

        update: (messages) ->
            @model = messages

        append_to: (parent) ->
            @$el.appendTo parent
