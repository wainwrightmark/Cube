
//using socket;

//using sys;

//using threading;

//using solver;

//using time;

//using System.Collections.Generic;

//using System;

//using System.Linq;

//public static class sockets {

//    // ################## The code of the server socket which communicates with the client ##################################
//    public static void client_thread(object conn, object maxlen, object timeout) {
//        object reply;
//        while (true) {
//            // infinite loop only necessary for telnet client
//            // Receiving from client
//            var data = new List<object>();
//            while (!(data.Contains(ord("\n")) || data.Contains(ord("\r")))) {
//                try {
//                    var a = conn.recv(1024).upper();
//                    if (a.Count == 0) {
//                        conn.close();
//                        Console.WriteLine("Connection closed", flush: true);
//                        return;
//                    }
//                } catch {
//                    Console.WriteLine("Connection closed", flush: true);
//                    conn.close();
//                    return;
//                }
//                foreach (var i in Enumerable.Range(0, a.Count)) {
//                    if (new List<int> {
//                        ord("\n"),
//                        ord("\r"),
//                        ord("G"),
//                        ord("E"),
//                        ord("T"),
//                        ord("U"),
//                        ord("R"),
//                        ord("FaceletPosition"),
//                        ord("D"),
//                        ord("L"),
//                        ord("B")
//                    }.Contains(a[i])) {
//                        data.append(a[i]);
//                    }
//                }
//            }
//            if (data[0] == ord("X")) {
//                break;
//            }
//            var defstr = "".join(from i in data
//                where chr(i) > chr(32)
//                select chr(i));
//            var qpos = defstr.find("GET");
//            if (qpos >= 0) {
//                // in this case we suppose the client is a webbrowser
//                defstr = defstr[(qpos  +  3)::(qpos  +  57)];
//                reply = "HTTP/1.1 200 OK" + "\n\n" + "<html><head><title>Answer from Cubesolver</title></head><body>" + "\n";
//                reply += solver.Solve(defstr, maxlen, timeout) + "\n" + "</body></html>" + "\n";
//                conn.sendall(reply.encode());
//                conn.close();
//            } else {
//                // other client, for example the GUI client or telnet
//                reply = (solver.Solve(defstr, maxlen, timeout) + "\n").encode();
//                Console.WriteLine(defstr);
//                try {
//                    conn.sendall(reply);
//                } catch {
//                    Console.WriteLine("Error while sending data. Connection closed", flush: true);
//                    conn.close();
//                    return;
//                }
//            }
//        }
//        conn.close();
//    }

//    public static object server_start(object args) {
//        var s = socket.socket(socket.AF_INET, socket.SOCK_STREAM);
//        Console.WriteLine("Server socket created");
//        try {
//            s.bind(("", Convert.ToInt32(args[1])));
//        } catch {
//            Console.WriteLine("Server socket bind failed. Error Code : " + e.errno.ToString());
//            sys.exit();
//        }
//        s.listen(10);
//        Console.WriteLine("Server now listening...");
//        while (1) {
//            var _tup_1 = s.accept();
//            var conn = _tup_1.Item1;
//            var addr = _tup_1.Item2;
//            Console.WriteLine("Connected with " + addr[0] + ":" + addr[1].ToString() + ", " + time.strftime("%Y.%m.%d  %H:%M:%S"));
//            threading.Thread(target: client_thread, args: (conn, Convert.ToInt32(args[2]), Convert.ToInt32(args[3]))).start();
//        }
//        s.close();
//    }
//}
