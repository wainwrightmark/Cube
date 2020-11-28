
//using socket;

//using Face;

//using MoveCubes;

//using Thread = threading.Thread;

//using grab_colors = vision2.grab_colors;

//using vision_params;

//using np = numpy;

//using System.Collections.Generic;

//using System.Linq;

//using System;

//public static class client_gui2 {

//    public static string DEFAULT_HOST = "localhost";

//    public static string DEFAULT_PORT = "8080";

//    public static int width = 60;

//    public static List<List<List<int>>> facelet_id = (from fc in Enumerable.Range(0, 6)
//        select (from row in Enumerable.Range(0, 3)
//            select (from col in Enumerable.Range(0, 3)
//                select 0).ToList()).ToList()).ToList();

//    public static List<int> colorpick_id = (from i in Enumerable.Range(0, 6)
//        select 0).ToList();

//    public static object curcol = null;

//    public static Tuple<string, string, string, string, string, string> t = ("U", "R", "FaceletPosition", "D", "L", "B");

//    public static Tuple<string, string, string, string, string, string> cols = ("yellow", "green", "red", "white", "blue", "orange");

//    //#######################################################################################################################
//    // ################################################ Diverse functions ###################################################
//    // Display messages.
//    public static void show_text(object txt) {
//        Console.WriteLine(txt);
//        display.insert(INSERT, txt);
//        root.update_idletasks();
//    }

//    // Initialize the facelet grid on the canvas.
//    public static void create_facelet_rects(int a) {
//        var offset = ((1, 0), (2, 1), (1, 1), (1, 2), (0, 1), (3, 1));
//        for(var f = 0; f < 6; f++) {
//            for(var row = 0; row < 3; row++) {
//                var y = 10 + offset[f][1] * 3 * a + row * a;
//                for(var col = 0; col < 3; col++) {
//                    var x = 10 + offset[f][0] * 3 * a + col * a;
//                    facelet_id[f][row][col] = canvas.create_rectangle(x, y, x + a, y + a, fill: "grey");
//                    if (row == 1 && col == 1) {
//                        canvas.create_text(x + width / 2, y + width / 2, font: ("", 14), text: t[f], state: DISABLED);
//                    }
//                }
//            }
//        }
//        for(var f = 0; f < 6; f++) {
//            canvas.itemconfig(facelet_id[f][1][1], fill: cols[f]);
//        }
//    }

//    // Initialize the "paintbox" on the canvas.
//    public static void create_colorpick_rects(int a) {
//        for(var i = 0; i < 6; i++) {
//            var x = i % 3 * (a + 5) + 7 * a;
//            var y = i / 3 * (a + 5) + 7 * a;
//            colorpick_id[i] = canvas.create_rectangle(x, y, x + a, y + a, fill: cols[i]);
//            canvas.itemconfig(colorpick_id[0], width: 4);
//            curcol = cols[0];
//        }
//    }

//    // Generate the cube definition string from the facelet colors.
//    public static void get_definition_string() {
//        var color_to_facelet = new Dictionary<object, object> {
//        };
//        for(var i = 0; i < 6; i++) {
//            color_to_facelet.update(new Dictionary<object, object> {
//                {
//                    canvas.itemcget(facelet_id[i][1][1], "fill"),
//                    t[i]}});
//        }
//        var s = "";
//        for(var f = 0; f < 6; f++) {
//            for(var row = 0; row < 3; row++) {
//                for(var col = 0; col < 3; col++) {
//                    s += color_to_facelet[canvas.itemcget(facelet_id[f][row][col], "fill")];
//                }
//            }
//        }
//        return s;
//    }

//    //#######################################################################################################################
//    // ############################### Solve the displayed cube with a local or remote server ###############################
//    // Connect to the server and return the solving maneuver.
//    public static object Solve() {
//        display.delete(1.0, END);
//        try {
//            var s = socket.socket(socket.AF_INET, socket.SOCK_STREAM);
//        } catch {
//            show_text("Failed to create socket");
//            return;
//        }
//        var host = txt_host.get(1.0, END).rstrip();
//        var port = Convert.ToInt32(txt_port.get(1.0, END));
//        try {
//            var remote_ip = socket.gethostbyname(host);
//        } catch {
//            show_text("Hostname could not be resolved.");
//            return;
//        }
//        try {
//            s.connect((remote_ip, port));
//        } catch {
//            show_text("Cannot connect to server!");
//            return;
//        }
//        show_text("Connected with " + remote_ip + "\n");
//        try {
//            var defstr = get_definition_string() + "\n";
//        } catch {
//            show_text("Invalid facelet configuration.\nWrong or missing colors.");
//            return;
//        }
//        show_text(defstr);
//        try {
//            s.sendall((defstr + "\n").encode());
//        } catch {
//            show_text("Cannot send cube configuration to server.");
//            return;
//        }
//        show_text(s.recv(2048).decode());
//    }

//    //#######################################################################################################################
//    // ################################# Functions to change the facelet colors #############################################
//    // Restore the cube to a clean cube.
//    public static void clean() {
//        for(var f = 0; f < 6; f++) {
//            for(var row = 0; row < 3; row++) {
//                for(var col = 0; col < 3; col++) {
//                    canvas.itemconfig(facelet_id[f][row][col], fill: canvas.itemcget(facelet_id[f][1][1], "fill"));
//                }
//            }
//        }
//    }

//    // Remove the facelet colors except the center facelets colors.
//    public static void empty() {
//        for(var f = 0; f < 6; f++) {
//            for(var row = 0; row < 3; row++) {
//                for(var col = 0; col < 3; col++) {
//                    if (row != 1 || col != 1) {
//                        canvas.itemconfig(facelet_id[f][row][col], fill: "grey");
//                    }
//                }
//            }
//        }
//    }

//    // Generate a random cube and sets the corresponding facelet colors.
//    public static void random() {
//        var cc = MutableCubieCube();
//        cc.randomize();
//        var fc = cc.to_facelet_cube();
//        var idx = 0;
//        for(var f = 0; f < 6; f++) {
//            for(var row = 0; row < 3; row++) {
//                for(var col = 0; col < 3; col++) {
//                    canvas.itemconfig(facelet_id[f][row][col], fill: cols[fc.f[idx]]);
//                    idx += 1;
//                }
//            }
//        }
//    }

//    //#######################################################################################################################
//    // ################################### Edit the facelet colors ##########################################################
//    // Define how to react on left mouse clicks
//    public static void click(object @event) {
//        var idlist = canvas.find_withtag("current");
//        if (idlist.Count > 0) {
//            if (colorpick_id.Contains(idlist[0])) {
//                curcol = canvas.itemcget("current", "fill");
//                for(var i = 0; i < 6; i++) {
//                    canvas.itemconfig(colorpick_id[i], width: 1);
//                }
//                canvas.itemconfig("current", width: 5);
//            } else {
//                canvas.itemconfig("current", fill: curcol);
//            }
//        }
//    }

//    //#######################################################################################################################
//    // ######################################### functions to set the slider values #########################################
//    public static void set_rgb_L(object val) {
//        vision_params.rgb_L = Convert.ToInt32(val);
//    }

//    public static void set_orange_L(object val) {
//        vision_params.orange_L = Convert.ToInt32(val);
//    }

//    public static void set_orange_H(object val) {
//        vision_params.orange_H = Convert.ToInt32(val);
//    }

//    public static void set_yellow_H(object val) {
//        vision_params.yellow_H = Convert.ToInt32(val);
//    }

//    public static void set_green_H(object val) {
//        vision_params.green_H = Convert.ToInt32(val);
//    }

//    public static void set_blue_H(object val) {
//        vision_params.blue_H = Convert.ToInt32(val);
//    }

//    public static void set_sat_W(object val) {
//        vision_params.sat_W = Convert.ToInt32(val);
//    }

//    public static void set_val_W(object val) {
//        vision_params.val_W = Convert.ToInt32(val);
//    }

//    public static void set_sigma_C(object val) {
//        vision_params.sigma_C = Convert.ToInt32(val);
//    }

//    public static void set_delta_C(object val) {
//        vision_params.delta_C = Convert.ToInt32(val);
//    }

//    // Transfer the facelet colors detected by the opencv vision to the GUI editor.
//    public static void transfer() {
//        if (vision_params.face_col.Count == 0) {
//            return;
//        }
//        var centercol = vision_params.face_col[1][1];
//        vision_params.cube_col[centercol] = vision_params.face_col;
//        vision_params.cube_hsv[centercol] = vision_params.face_hsv;
//        var dc = new Dictionary<object, object> {
//        };
//        for(var i = 0; i < 6; i++) {
//            dc[canvas.itemcget(facelet_id[i][1][1], "fill")] = i;
//        }
//        for(var i = 0; i < 3; i++) {
//            for(var j = 0; j < 3; j++) {
//                canvas.itemconfig(facelet_id[dc[centercol]][i][j], fill: vision_params.face_col[i][j]);
//            }
//        }
//    }

//    public static object root = Tk();

//    static client_gui2() {
//        root.wm_title("Solver Client");
//        canvas.pack();
//        txt_host.insert(INSERT, DEFAULT_HOST);
//        txt_port.insert(INSERT, DEFAULT_PORT);
//        canvas.bind("<Button-1>", click);
//        create_facelet_rects(width);
//        create_colorpick_rects(width);
//        canvas.create_window(10, 12 + 6.0 * width, anchor: NW, window: s_orange_L);
//        s_orange_L.set(vision_params.orange_L);
//        canvas.create_window(10, 12 + 6.6 * width, anchor: NW, window: s_orange_H);
//        s_orange_H.set(vision_params.orange_H);
//        canvas.create_window(10, 12 + 7.2 * width, anchor: NW, window: s_yellow_H);
//        s_yellow_H.set(vision_params.yellow_H);
//        canvas.create_window(10, 12 + 7.8 * width, anchor: NW, window: s_green_H);
//        s_green_H.set(vision_params.green_H);
//        canvas.create_window(10, 12 + 8.4 * width, anchor: NW, window: s_blue_H);
//        s_blue_H.set(vision_params.blue_H);
//        canvas.create_window(10 + width * 1.5, 12 + 6 * width, anchor: NW, window: s_rgb_L);
//        s_rgb_L.set(vision_params.rgb_L);
//        canvas.create_window(10 + width * 1.5, 12 + 6.6 * width, anchor: NW, window: s_sat_W);
//        s_sat_W.set(vision_params.sat_W);
//        canvas.create_window(10 + width * 1.5, 12 + 7.2 * width, anchor: NW, window: s_val_W);
//        s_val_W.set(vision_params.val_W);
//        canvas.create_window(10 + width * 1.5, 12 + 7.8 * width, anchor: NW, window: s_sigma_C);
//        s_sigma_C.set(vision_params.sigma_C);
//        canvas.create_window(10 + width * 1.5, 12 + 8.4 * width, anchor: NW, window: s_delta_C);
//        s_delta_C.set(vision_params.delta_C);
//        canvas.create_window(10 + 0.5 * width, 10 + 2.1 * width, anchor: NW, window: btransfer);
//        root.mainloop();
//    }

//    public static object canvas = Canvas(root, width: 12 * width + 20, height: 9 * width + 20);

//    public static object bsolve = Button(text: "Solve", height: 2, width: 10, relief: RAISED, command: Solve);

//    public static object bsolve_window = canvas.create_window(10 + 10.5 * width, 10 + 6.5 * width, anchor: NW, window: bsolve);

//    public static object bclean = Button(text: "Clean", height: 1, width: 10, relief: RAISED, command: clean);

//    public static object bclean_window = canvas.create_window(10 + 10.5 * width, 10 + 7.5 * width, anchor: NW, window: bclean);

//    public static object bempty = Button(text: "Empty", height: 1, width: 10, relief: RAISED, command: empty);

//    public static object bempty_window = canvas.create_window(10 + 10.5 * width, 10 + 8 * width, anchor: NW, window: bempty);

//    public static object brandom = Button(text: "Random", height: 1, width: 10, relief: RAISED, command: random);

//    public static object brandom_window = canvas.create_window(10 + 10.5 * width, 10 + 8.5 * width, anchor: NW, window: brandom);

//    public static object display = Text(height: 7, width: 39);

//    public static object text_window = canvas.create_window(10 + 6.5 * width, 10 + 0.5 * width, anchor: NW, window: display);

//    public static object hp = Label(text: "    Hostname and Port");

//    public static object hp_window = canvas.create_window(10 + 0 * width, 10 + 0.6 * width, anchor: NW, window: hp);

//    public static object txt_host = Text(height: 1, width: 20);

//    public static object txt_host_window = canvas.create_window(10 + 0 * width, 10 + 1 * width, anchor: NW, window: txt_host);

//    public static object txt_port = Text(height: 1, width: 20);

//    public static object txt_port_window = canvas.create_window(10 + 0 * width, 10 + 1.5 * width, anchor: NW, window: txt_port);

//    public static object s_orange_L = Scale(root, from_: 1, to: 14, length: width * 1.4, showvalue: 0, label: "red-orange", orient: HORIZONTAL, command: set_orange_L);

//    public static object s_orange_H = Scale(root, from_: 8, to: 40, length: width * 1.4, showvalue: 0, label: "orange-yellow", orient: HORIZONTAL, command: set_orange_H);

//    public static object s_yellow_H = Scale(root, from_: 31, to: 80, length: width * 1.4, showvalue: 0, label: "yellow-green", orient: HORIZONTAL, command: set_yellow_H);

//    public static object s_green_H = Scale(root, from_: 70, to: 120, length: width * 1.4, showvalue: 0, label: "green-blue", orient: HORIZONTAL, command: set_green_H);

//    public static object s_blue_H = Scale(root, from_: 120, to: 180, length: width * 1.4, showvalue: 0, label: "blue-red", orient: HORIZONTAL, command: set_blue_H);

//    public static object s_rgb_L = Scale(root, from_: 0, to: 140, length: width * 1.4, showvalue: 0, label: "black-filter", orient: HORIZONTAL, command: set_rgb_L);

//    public static object s_sat_W = Scale(root, from_: 120, to: 0, length: width * 1.4, showvalue: 0, label: "white-filter s", orient: HORIZONTAL, command: set_sat_W);

//    public static object s_val_W = Scale(root, from_: 80, to: 255, length: width * 1.4, showvalue: 0, label: "white-filter v", orient: HORIZONTAL, command: set_val_W);

//    public static object s_sigma_C = Scale(root, from_: 30, to: 0, length: width * 1.4, showvalue: 0, label: "color-filter \u03c3", orient: HORIZONTAL, command: set_sigma_C);

//    public static object s_delta_C = Scale(root, from_: 10, to: 0, length: width * 1.4, showvalue: 0, label: "color-filter \u03b4", orient: HORIZONTAL, command: set_delta_C);

//    public static object btransfer = Button(text: "Webcam import", height: 2, width: 13, relief: RAISED, command: transfer);
//}
