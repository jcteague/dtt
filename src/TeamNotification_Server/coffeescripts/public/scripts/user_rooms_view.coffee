define 'user_rooms_view', ['general_view','messages_view','form_view','form_template_renderer'], (GeneralView, MessagesView, FormView, FormTemplateRenderer) ->

    class UserRoomsView extends GeneralView
        
        id: 'user-rooms-container'
        initialize: ->
            console.log 'asd'
            @formTemplateRenderer = new FormTemplateRenderer()
            @data = @model.attributes
        render: ->
            @$el.empty()
            navInner = $("<div>",{"class":"navbar-inner"})
            navContent = $("<div>",{"class":"container-fluid"})
            #navContent.append "<a class=​'btn btn-navbar' data-toggle=​'collapse' data-target=​'.nav-collapse'>​​<span class='icon-bar'></span><span class='icon-bar'></span><span class='icon-bar'></span></a>​"
            userName = $("<a>", { "class":"brand", "href":"/client#/user/#{@data.user.user_id}/"})
            userName.append @data.user.name
            #divCollapse = $("<div>",{"class":"nav-collapse"})
            @$el.attr('class', 'navbar navbar-fixed-top')
            ul = $("<ul>",{"class":"nav pull-right"})
            ul.append "<li class='divider-vertical'></li>"
            userRooms = $("<li>",{'class':'dropdown'})
            userRooms.append "<a href='#' id='DropDownInfuncional' class='dropdown-toggle' data-toggle='dropdown'>Rooms <b class='caret'></b></a>"
            
            userRoomsList = $("<ul>",{"class":"dropdown-menu"})
            
            for room in @data.user_rooms
                userRoomsList.append "<li><a href='client#/room/#{room.room_id}/messages'>#{room.name}</a></li>"
            
            userRooms.append userRoomsList
            ul.append userRooms
            navContent.append userName
            navContent.append ul
            navInner.append navContent
            $(userRooms).bind 'click', () ->
               $(userRoomsList).toggle() 
            @$el.append navInner
            @
        append_to: (parent) ->
            @$el.appendTo parent
