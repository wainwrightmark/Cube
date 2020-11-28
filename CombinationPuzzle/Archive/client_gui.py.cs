

//using static Face;

//using static MoveCubes;

//using System.Collections.Generic;

//using System.Linq;

//using System;

//public static class client_gui {

//    public static string DEFAULT_HOST = "localhost";

//    public static string DEFAULT_PORT = "8080";

//    public static int width = 60;

//    public static List<List<List<int>>> facelet_id = (from face in Enumerable.Range(0, 6)
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

//    // Generate a random cube and set the corresponding facelet colors.
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
//    // Define how to react on left mouse clicks.
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

//    public static object root = Tk();

//    static client_gui() {
//        root.wm_title("Solver Client");
//        canvas.pack();
//        txt_host.insert(INSERT, DEFAULT_HOST);
//        txt_port.insert(INSERT, DEFAULT_PORT);
//        canvas.bind("<Button-1>", click);
//        create_facelet_rects(width);
//        create_colorpick_rects(width);
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
//}
