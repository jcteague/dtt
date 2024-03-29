define 'server_response_view', ['backbone'], (Backbone) ->

    class ServerResponseView extends Backbone.View

        id: 'server-response-container'

        render: ->
            @$el.empty()
            if @model?
                @$el.append("""<div class="alert alert-info" id='#notification'><button type="button" class="close" data-dismiss="alert">x</button>"""+message+"""</div>""") for message in @model
            @

        update: (messages) ->
            @model = messages

        append_to: (parent) ->
            @$el.appendTo parent

        clear: ->
            @model = null
