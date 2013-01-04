define 'user_rooms_view', ['general_view','messages_view','form_view','form_template_renderer'], (GeneralView, MessagesView, FormView, FormTemplateRenderer) ->

    class UserRoomsView extends GeneralView

        id: 'user-rooms-container'

        initialize: ->
            @formTemplateRenderer = new FormTemplateRenderer()
            @data = @model.attributes

        render: ->
            @$el.empty()
            navInner = $("<div>",{"class":"navbar-inner"})
            navContent = $("<div>",{"class":"container-fluid"})
            userName = $("<a>", { "class":"brand", "href":"/#/user/#{@data.user.user_id}/"})
            userName.append @data.user.name
            roomName = $("<span/>", {"class": "room-name pull-left muted"}).append "(#{@model.get('room').name})"
            @$el.attr('class', 'navbar navbar-fixed-top')
            ul = $("<ul>",{"class":"nav pull-right"})
            ul.append "<li class='divider-vertical'></li>"
            userRooms = $("<li>",{'class':'dropdown'})
            userRooms.append "<a href='#' id='DropDownInfuncional' class='dropdown-toggle' data-toggle='dropdown'>Rooms <b class='caret'></b></a>"
            userRoomsList = $("<ul>",{"class":"dropdown-menu"})
            for room in @data.user_rooms
                userRoomsList.append "<li><a href='/#/room/#{room.room_id}/messages'>#{room.name}</a></li>"
            
            userRooms.append userRoomsList
            ul.append userRooms
            navContent.append userName
            navContent.append roomName
            navContent.append ul
            navInner.append navContent
            $(userRooms).bind 'click', () ->
               $(userRoomsList).toggle() 
            @$el.append navInner
            @
        append_to: (parent) ->
            @$el.appendTo parent
