define 'messages_view', ['backbone'], (Backbone) ->

    class MessagesView extends Backbone.View

        id: 'messages-container'

        render: ->
            @$el.empty()
            @$el.append "<p>#{message}</p>" for message in @model
            @

        update: (messages) ->
            @model = messages

        append_to: (parent) ->
            @$el.appendTo parent
