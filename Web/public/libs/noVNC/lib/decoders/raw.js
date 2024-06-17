"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports["default"] = void 0;
function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }
function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, _toPropertyKey(descriptor.key), descriptor); } }
function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }
function _toPropertyKey(t) { var i = _toPrimitive(t, "string"); return "symbol" == _typeof(i) ? i : i + ""; }
function _toPrimitive(t, r) { if ("object" != _typeof(t) || !t) return t; var e = t[Symbol.toPrimitive]; if (void 0 !== e) { var i = e.call(t, r || "default"); if ("object" != _typeof(i)) return i; throw new TypeError("@@toPrimitive must return a primitive value."); } return ("string" === r ? String : Number)(t); }
/*
 * noVNC: HTML5 VNC client
 * Copyright (C) 2019 The noVNC Authors
 * Licensed under MPL 2.0 (see LICENSE.txt)
 *
 * See README.md for usage and integration instructions.
 *
 */
var RawDecoder = exports["default"] = /*#__PURE__*/function () {
  function RawDecoder() {
    _classCallCheck(this, RawDecoder);
    this._lines = 0;
  }
  return _createClass(RawDecoder, [{
    key: "decodeRect",
    value: function decodeRect(x, y, width, height, sock, display, depth) {
      if (width === 0 || height === 0) {
        return true;
      }
      if (this._lines === 0) {
        this._lines = height;
      }
      var pixelSize = depth == 8 ? 1 : 4;
      var bytesPerLine = width * pixelSize;
      while (this._lines > 0) {
        if (sock.rQwait("RAW", bytesPerLine)) {
          return false;
        }
        var curY = y + (height - this._lines);
        var data = sock.rQshiftBytes(bytesPerLine, false);

        // Convert data if needed
        if (depth == 8) {
          var newdata = new Uint8Array(width * 4);
          for (var i = 0; i < width; i++) {
            newdata[i * 4 + 0] = (data[i] >> 0 & 0x3) * 255 / 3;
            newdata[i * 4 + 1] = (data[i] >> 2 & 0x3) * 255 / 3;
            newdata[i * 4 + 2] = (data[i] >> 4 & 0x3) * 255 / 3;
            newdata[i * 4 + 3] = 255;
          }
          data = newdata;
        }

        // Max sure the image is fully opaque
        for (var _i = 0; _i < width; _i++) {
          data[_i * 4 + 3] = 255;
        }
        display.blitImage(x, curY, width, 1, data, 0);
        this._lines--;
      }
      return true;
    }
  }]);
}();