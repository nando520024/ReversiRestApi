"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var Game = function (url) {
  //Configuratie en state waarden
  var configMap = {
    apiUrl: url
  };
  var stateMap = {
    gameState: 0
  };

  var _getCurrentGameState = function _getCurrentGameState() {
    stateMap.gameState = Game.Model.getGameState();
  }; // Private function init


  var privateInit = function privateInit(callbackFunction) {
    // console.log(configMap.apiUrl);
    console.log("Init van Game");
    Game.Model.init();
    Game.Reversi.init();
    Game.Data.init("development");
    setInterval(_getCurrentGameState, 2000);
    callbackFunction();
    return true;
  }; // Waarde/object geretourneerd aan de outer scope


  return {
    init: privateInit
  };
}('/api/url');

$(document).ready(function () {
  console.log('ready!'); // Add click event

  $('#btn-ok').on('click', function () {
    alert('The ok button was clicked.');
    var feedbackWidget = new FeedbackWidget('#feedback');
    feedbackWidget.show('Mikeeeeeeeeeeeeeeee wil deelnemen aan jouw spel. Geef akkoord.', 'success');
  });
  $('#btn-no').on('click', function () {
    alert('The no button was clicked.');
    var feedbackWidget = new FeedbackWidget('#feedback');
    feedbackWidget.hide();
  });
  $('#btn-yes').on('click', function () {
    alert('The yes button was clicked.');
    var feedbackWidget = new FeedbackWidget('#feedback');
    feedbackWidget.hide();
  });
  $('#btn-exit').on('click', function () {
    alert('The exit button was clicked.');
    var feedbackWidget = new FeedbackWidget('#feedback');
    feedbackWidget.hide();
  }); // btn-yes bibber effect toggles the class bibber every 2 seconds

  $(document).ready(function () {
    setInterval(function () {
      $("#btn-yes").toggleClass('shake');
    }, 2000);
  });
});

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
  }

  _createClass(FeedbackWidget, [{
    key: "elementId",
    get: function get() {
      return this._elementId;
    }
  }, {
    key: "show",
    value: function show(message, type) {
      // Add fade in and remove fade out
      $('#feedback').removeClass('fade-out');
      $('#feedback').addClass('fade-in'); // Show the feedbackwidget

      var x = document.querySelector(this.elementId);
      x.style.display = 'block'; // Set the text of the feedback

      $('#feedback__text').text(message); // Check if type is success, then alert-success, if type is danger, then alert-danger

      if (type === 'success') {
        $(this.elementId).removeClass('alert-danger');
        $(this.elementId).addClass('alert-success');
      } else if (type === 'danger') {
        $(this.elementId).removeClass('alert-success');
        $(this.elementId).addClass('alert-danger');
      } // Call method log and give an object with message and type


      this.log({
        message: message,
        type: type
      });
    }
  }, {
    key: "hide",
    value: function hide() {
      // add fade out and remove fade in
      $('#feedback').removeClass('fade-in');
      $('#feedback').addClass('fade-out'); // Hide the feedbackwidget

      var x = document.querySelector(this.elementId); // wait 5 sec so the animation can end

      setTimeout(function () {
        x.style.display = 'none';
      }, 2500);
    }
  }, {
    key: "log",
    value: function log(message) {
      // Get messages from localStorage or make a new JSON object
      var localStorageMessages = JSON.parse(localStorage.getItem('feedback_widget')) || {
        messages: []
      }; // Add the message at index 0

      localStorageMessages.messages.unshift(message); // Check if the messages is bigger then 10

      if (localStorageMessages.messages.length > 10) {
        // Remove the last message from the array
        localStorageMessages.messages.pop();
      } // Set the localstorage with key 'feedback_widget' and the new message


      localStorage.setItem('feedback_widget', JSON.stringify(localStorageMessages));
    }
  }, {
    key: "removeLog",
    value: function removeLog() {
      // Clear the log with key 'feedback_widget'
      localStorage.removeItem('feedback_widget');
    }
  }, {
    key: "history",
    value: function history() {
      // Get messages from localStorage or make a new JSON object
      var localStorageMessages = JSON.parse(localStorage.getItem('feedback_widget')) || {
        messages: []
      }; // Show the messages in the console

      localStorageMessages.messages.forEach(function (element) {
        return console.log(element.type + ' - ' + element.message + ' ');
      });
    }
  }]);

  return FeedbackWidget;
}();

Game.Data = function () {
  var configMap = {
    apiKey: "7fa35c872e1d1bf312fc86d652fa4965",
    // OpenWeatherApi key
    mock: [{
      url: "api/Spel/Beurt",
      data: 0
    }]
  };
  var stateMap = {
    environment: 'development'
  };

  var getMockData = function getMockData(url) {
    var mockData = configMap.mock;
    return new Promise(function (resolve, reject) {
      resolve(mockData);
    });
  };

  var get = function get(url) {
    // Returns the data from the OpenWeatherApi
    if (stateMap.environment === "production") {
      return $.get(url + configMap.apiKey).then(function (r) {
        return r;
      })["catch"](function (e) {
        console.log(e.message);
      });
    } else if (stateMap.environment === "development") {
      return getMockData(url);
    }
  };

  var privateInit = function privateInit(environment) {
    stateMap.environment = environment;

    switch (stateMap.environment) {
      case "development":
        break;

      case "production":
        break;

      default:
        throw new Error("Verkeerde environment gegeven");
    }

    console.log("Init van Game Data");
  };

  return {
    init: privateInit,
    get: get
  };
}();

Game.Model = function () {
  var configMap = {};

  var _getGameState = function _getGameState() {
    var gameState; //aanvraag via Game.Data

    Game.Data.get("/api/Spel/Beurt/tokentje").then(function (data) {
      gameState = data[0].data; //controle of ontvangen data valide is

      switch (gameState) {
        case 0:
          console.log("game state heeft geen specifieke waarde");
          break;

        case 1:
          console.log("game state wit aan zet");
          break;

        case 2:
          console.log("game state zwart aan zet");
          break;

        default:
          throw new Error("game state is niet geldig");
      }

      return gameState;
    });
  };

  var privateInit = function privateInit() {
    console.log("Init van Game Model");
  };

  var _getWeather = function _getWeather() {
    var url = "http://api.openweathermap.org/data/2.5/weather?q=zwolle&apikey=";
    Game.Data.get(url).then(function (data) {
      if (data.main.temp === null) {
        console.log("temperatuur niet meegegeven");
      } else {
        console.log("temperatuur meegegeven");
        console.log(data.main.temp);
      }
    });
  };

  return {
    init: privateInit,
    getWeather: _getWeather,
    getGameState: _getGameState
  };
}();

Game.Reversi = function () {
  var configMap = {};

  var privateInit = function privateInit() {
    console.log("Init van Game Reversi");
  };

  return {
    init: privateInit
  };
}();