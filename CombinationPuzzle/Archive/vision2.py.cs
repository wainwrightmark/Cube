
//using cv2;

//using np = numpy;

//using vision_params;

//using System;

//using System.Linq;

//using System.Collections.Generic;

//public static class vision2 {

//    public static int grid_N = 25;

//    // Draw grid onto the webcam output. Only used for debugging purposes.
//    public static void drawgrid(object img, object n) {
//        var _tup_1 = img.shape[::2];
//        var h = _tup_1.Item1;
//        var w = _tup_1.Item2;
//        var sz = h / n;
//        var border = 1 * sz;
//        foreach (var y in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(h - border - border) / sz))).Select(_x_1 => border + _x_1 * sz)) {
//            foreach (var x in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(w - border - border) / sz))).Select(_x_2 => border + _x_2 * sz)) {
//                cv2.rectangle(img, (x, y), (x + sz, y + sz), (0, 0, 0), 1);
//                cv2.rectangle(img, (x - 1, y - 1), (x + 1 + sz, y + 1 + sz), (255, 255, 255), 1);
//            }
//        }
//    }

//    // Delete one of two potential facelet centers stored in pts if they are too close to each other.
//    public static void del_duplicates(object pts) {
//        var delta = width / 12;
//        var dele = true;
//        while (dele) {
//            dele = false;
//            var r = Enumerable.Range(0, pts.Count);
//            foreach (var i in r) {
//                foreach (var j in r[i + 1]) {
//                    if (np.linalg.norm(pts[i] - pts[j]) < delta) {
//                        pts.Remove(j);
//                        dele = true;
//                    }
//                    if (dele) {
//                        break;
//                    }
//                }
//                if (dele) {
//                    break;
//                }
//            }
//        }
//    }

//    // The mediod is the point with the smallest summed distance from the other points.
//    //     This is a candidate for the center facelet.
//    public static void medoid(object pts) {
//        var res = np.array(new List<double> {
//            0.0,
//            0.0
//        });
//        var smin = 100000;
//        foreach (var i in pts) {
//            var s = 0;
//            foreach (var j in pts) {
//                s += np.linalg.norm(i - j);
//            }
//            if (s < smin) {
//                smin = s;
//                res = i;
//            }
//        }
//        return res;
//    }

//    // Separate the candidates into edge and corner facelets by their distance from the medoid.
//    public static Tuple<List<object>, List<object>> facelets(object pts, void med) {
//        object d;
//        var ed = new List<object>();
//        var CornerOrientations = new List<object>();
//        if (med[0] == 0) {
//            return Tuple.Create(CornerOrientations, ed);
//        }
//        // find shortest distance
//        var dmin = 10000;
//        foreach (var p in pts) {
//            d = np.linalg.norm(p - med);
//            if (1 < d < dmin) {
//                dmin = d;
//            }
//        }
//        // edgefacelets should be in a distance not more than dmin*1.3
//        foreach (var p in pts) {
//            d = np.linalg.norm(p - med);
//            if (dmin - 1 < d < dmin * 1.3) {
//                ed.append(p);
//            }
//        }
//        // now find the corner facelets
//        foreach (var p in pts) {
//            d = np.linalg.norm(p - med);
//            if (dmin * 1.3 < d < dmin * 1.7) {
//                CornerOrientations.append(p);
//            }
//        }
//        return Tuple.Create(CornerOrientations, ed);
//    }

//    // If we have detected a facelet position, the point reflection at the center also gives a facelet position.
//    //      We can use this position in case the other facelet was not detected directly.
//    public static Tuple<List<object>, List<object>> mirr_facelet(object CornerOrientations, object ed, void med) {
//        object pa;
//        var aef = new List<object>();
//        var acf = new List<object>();
//        foreach (var p in ed) {
//            pa = 2 * med - p;
//            aef.append(pa);
//        }
//        foreach (var p in CornerOrientations) {
//            pa = 2 * med - p;
//            acf.append(pa);
//        }
//        // delete duplicates
//        var delta = width / 12;
//        foreach (var k in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(-1 - (aef.Count - 1)) / -1))).Select(_x_1 => aef.Count - 1 + _x_1 * -1)) {
//            foreach (var p in ed) {
//                if (np.linalg.norm(aef[k] - p) < delta) {
//                    aef.Remove(k);
//                    break;
//                }
//            }
//        }
//        foreach (var k in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(-1 - (acf.Count - 1)) / -1))).Select(_x_2 => acf.Count - 1 + _x_2 * -1)) {
//            foreach (var p in CornerOrientations) {
//                if (np.linalg.norm(acf[k] - p) < delta) {
//                    acf.Remove(k);
//                    break;
//                }
//            }
//        }
//        return Tuple.Create(acf, aef);
//    }

//    // Display the colornames on the webcam picture.
//    public static void display_colorname(object bgrcap, void p) {
//        object txtcol;
//        p = p.astype(np.uint16);
//        var _tup_1 = getcolor(p);
//        var col = _tup_1.Item2;
//        if (("blue", "green", "red").Contains(col)) {
//            txtcol = (255, 255, 255);
//        } else {
//            txtcol = (0, 0, 0);
//        }
//        var font = cv2.FONT_HERSHEY_SIMPLEX;
//        var tz = cv2.getTextSize(col, font, 0.4, 1)[0];
//        cv2.putText(bgrcap, col, tuple(p - (tz[0] / 2, -tz[1] / 2)), font, 0.4, txtcol, 1);
//    }

//    // Decide the color of a facelet by its h value (non white) or by s and v (white).
//    public static Tuple<int, string> getcolor(List<List<object>> p) {
//        var sz = 10;
//        p = p.astype(np.uint16);
//        var rect = hsv[(p[1]  -  sz)::(p[1]  +  sz),(p[0]  -  sz)::(p[0]  +  sz)];
//        var median = np.sum(rect, axis: (0, 1)) / sz / sz / 4;
//        var _tup_1 = median;
//        var mh = _tup_1.Item1;
//        var ms = _tup_1.Item2;
//        var mv = _tup_1.Item3;
//        if (ms <= vision_params.sat_W && mv >= vision_params.val_W) {
//            return Tuple.Create(median, "white");
//        } else if (vision_params.orange_L <= mh < vision_params.orange_H) {
//            return Tuple.Create(median, "orange");
//        } else if (vision_params.orange_H <= mh < vision_params.yellow_H) {
//            return Tuple.Create(median, "yellow");
//        } else if (vision_params.yellow_H <= mh < vision_params.green_H) {
//            if (ms < 150) {
//                return Tuple.Create(median, "white");
//            } else {
//                return Tuple.Create(median, "green");
//            }
//        } else if (vision_params.green_H <= mh < vision_params.blue_H) {
//            if (ms < 150) {
//                return Tuple.Create(median, "white");
//            } else {
//                return Tuple.Create(median, "blue");
//            }
//        } else {
//            return Tuple.Create(median, "red");
//        }
//    }

//    // Find the colors of the 9 facelets and decide their position on the cube Face.
//    public static object getcolors(
//        object CornerOrientations,
//        object ed,
//        object aco,
//        object aed,
//        void m) {
//        var centers = (from x in Enumerable.Range(0, 3)
//            select (from x in Enumerable.Range(0, 3)
//                select m).ToList()).ToList();
//        var colors = (from x in Enumerable.Range(0, 3)
//            select (from x in Enumerable.Range(0, 3)
//                select "").ToList()).ToList();
//        var s = np.array(new List<double> {
//            0.0,
//            0.0,
//            0.0
//        });
//        var hsvs = (from x in Enumerable.Range(0, 3)
//            select (from x in Enumerable.Range(0, 3)
//                select s).ToList()).ToList();
//        var cocents = CornerOrientations + aco;
//        if (cocents.Count != 4) {
//            return Tuple.Create(new List<object>(), new List<object>());
//        }
//        var edcents = ed + aed;
//        if (edcents.Count != 4) {
//            return Tuple.Create(new List<object>(), new List<object>());
//        }
//        foreach (var i in cocents) {
//            if (i[0] < m[0] && i[1] < m[1]) {
//                centers[0][0] = i;
//            } else if (i[0] > m[0] && i[1] < m[1]) {
//                centers[0][2] = i;
//            } else if (i[0] < m[0] && i[1] > m[1]) {
//                centers[2][0] = i;
//            } else if (i[0] > m[0] && i[1] > m[1]) {
//                centers[2][2] = i;
//            }
//        }
//        foreach (var i in edcents) {
//            if (i[1] < centers[0][1][1]) {
//                centers[0][1] = i;
//            }
//        }
//        foreach (var i in edcents) {
//            if (i[0] < centers[1][0][0]) {
//                centers[1][0] = i;
//            }
//        }
//        foreach (var i in edcents) {
//            if (i[0] > centers[1][2][0]) {
//                centers[1][2] = i;
//            }
//        }
//        foreach (var i in edcents) {
//            if (i[1] > centers[2][1][1]) {
//                centers[2][1] = i;
//            }
//        }
//        for(var x = 0; x < 3; x++) {
//            for(var y = 0; y < 3; y++) {
//                var _tup_1 = getcolor(centers[x][y]);
//                var hsv_ = _tup_1.Item1;
//                var col = _tup_1.Item2;
//                colors[x][y] = col;
//                hsvs[x][y] = hsv_;
//            }
//        }
//        return Tuple.Create(hsvs, colors);
//    }

//    //  Find the positions of squares in the webcam picture.
//    public static void find_squares(object bgrcap, int n) {
//        object rect3x3;
//        var _tup_1 = cv2.split(hsv);
//        var h = _tup_1.Item1;
//        var s = _tup_1.Item2;
//        var v = _tup_1.Item3;
//        var h_sqr = np.square(h);
//        var sz = height / n;
//        var border = 1 * sz;
//        var varmax_edges = 20;
//        // iterate all grid squares
//        foreach (var y in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(height - border - border) / sz))).Select(_x_1 => border + _x_1 * sz)) {
//            foreach (var x in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(width - border - border) / sz))).Select(_x_2 => border + _x_2 * sz)) {
//                // compute the standard deviation sigma of the hue in the square
//                var rect_h = h[y::(y  +  sz),x::(x  +  sz)];
//                var rect_h_sqr = h_sqr[y::(y  +  sz),x::(x  +  sz)];
//                var median_h = np.sum(rect_h) / sz / sz;
//                var sqr_median_h = median_h * median_h;
//                var median_h_sqr = np.sum(rect_h_sqr) / sz / sz;
//                var var = median_h_sqr - sqr_median_h;
//                var sigma = np.sqrt(var);
//                var delta = vision_params.delta_C;
//                // if sigma is small enough define a mask on the 3x3 square with the grid square in it's center
//                if (sigma < vision_params.sigma_W) {
//                    rect3x3 = hsv[(y  -  (1  *  sz))::(y  +  (2  *  sz)),(x  -  (1  *  sz))::(x  +  (2  *  sz))];
//                    mask = cv2.inRange(rect3x3, (0, 0, vision_params.val_W), (255, vision_params.sat_W, 255));
//                }
//                // and OR it to the white_mask
//                white_mask[(y  -  (1  *  sz))::(y  +  (2  *  sz)),(x  -  (1  *  sz))::(x  +  (2  *  sz))] = cv2.bitwise_or(mask, white_mask[(y  -  (1  *  sz))::(y  +  (2  *  sz)),(x  -  (1  *  sz))::(x  +  (2  *  sz))]);
//                // similar procedure for the color mask. Some issues because hues are computed modulo 180
//                if (sigma < vision_params.sigma_C) {
//                    rect3x3 = h[(y  -  (1  *  sz))::(y  +  (2  *  sz)),(x  -  (1  *  sz))::(x  +  (2  *  sz))];
//                    if (median_h + delta >= 180) {
//                        mask = cv2.inRange(rect3x3, 0, median_h + delta - 180);
//                        mask = cv2.bitwise_or(mask, cv2.inRange(rect3x3, median_h - delta, 180));
//                    } else if (median_h - delta < 0) {
//                        mask = cv2.inRange(rect3x3, median_h - delta + 180, 180);
//                        mask = cv2.bitwise_or(mask, cv2.inRange(rect3x3, 0, median_h + delta));
//                    } else {
//                        mask = cv2.inRange(rect3x3, median_h - delta, median_h + delta);
//                    }
//                    color_mask[(y  -  (1  *  sz))::(y  +  (2  *  sz)),(x  -  (1  *  sz))::(x  +  (2  *  sz))] = cv2.bitwise_or(mask, color_mask[(y  -  (1  *  sz))::(y  +  (2  *  sz)),(x  -  (1  *  sz))::(x  +  (2  *  sz))]);
//                }
//            }
//        }
//        black_mask = cv2.inRange(bgrcap, (0, 0, 0), (vision_params.rgb_L, vision_params.rgb_L, vision_params.rgb_L));
//        black_mask = cv2.bitwise_not(black_mask);
//        color_mask = cv2.bitwise_and(color_mask, black_mask);
//        color_mask = cv2.blur(color_mask, (20, 20));
//        color_mask = cv2.inRange(color_mask, 240, 255);
//        white_mask = cv2.bitwise_and(white_mask, black_mask);
//        white_mask = cv2.blur(white_mask, (20, 20));
//        white_mask = cv2.inRange(white_mask, 240, 255);
//        var itr = iter(new List<object> {
//            white_mask,
//            color_mask
//        });
//        // search for squares in the white_mask and in the color_mask
//        foreach (var j in itr) {
//            // find contours
//            // works for OpenCV 3.2 or higher. For versions < 3.2 omit im2 in the line below.
//            var _tup_2 = cv2.findContours(j, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE);
//            var im2 = _tup_2.Item1;
//            var contours = _tup_2.Item2;
//            var hierarchy = _tup_2.Item3;
//            foreach (var n in Enumerable.Range(0, contours.Count)) {
//                var approx = cv2.approxPolyDP(contours[n], sz / 2, true);
//                // if the contour cannot be approximated by a quadrangle it is not a facelet square
//                if (approx.shape[0] != 4) {
//                    continue;
//                }
//                var corners = approx[":",0];
//                // the edges of the square should have all about the same length
//                var edges = np.array(new List<object> {
//                    cv2.norm(corners[0] - corners[1], cv2.NORM_L2),
//                    cv2.norm(corners[1] - corners[2], cv2.NORM_L2),
//                    cv2.norm(corners[2] - corners[3], cv2.NORM_L2),
//                    cv2.norm(corners[3] - corners[0], cv2.NORM_L2)
//                });
//                var edges_mean_sq = Math.Pow(np.sum(edges) / 4, 2);
//                var edges_sq_mean = np.sum(np.square(edges)) / 4;
//                if (edges_sq_mean - edges_mean_sq > varmax_edges) {
//                    continue;
//                }
//                // cv2.drawContours(bgrcap, [approx], -1, (0, 0, 255), 8)
//                var middle = np.sum(corners, axis: 0) / 4;
//                cent.append(np.asarray(middle));
//            }
//        }
//    }

//    // Find the cube in the webcam picture and grab the colors of the facelets.
//    public static void grab_colors() {
//        var cap = cv2.VideoCapture(0);
//        var _tup_1 = cap.read();
//        var bgrcap = _tup_1.Item2;
//        if (bgrcap == null) {
//            Console.WriteLine("Cannot connect to webcam!");
//            Console.WriteLine("If you use a Raspberry Pi and no USB-webcam you have to Run \"sudo modprobe bvm2835-v4l2\" first!");
//            return;
//        }
//        var _tup_2 = bgrcap.shape[::2];
//        height = _tup_2.Item1;
//        width = _tup_2.Item2;
//        while (1) {
//            // Take each frame
//            var _tup_3 = cap.read();
//            bgrcap = _tup_3.Item2;
//            bgrcap = cv2.blur(bgrcap, (5, 5));
//            // now set all hue values >160 to 0. This is important since the color red often contains hue values
//            // in this range *and* also hue values >0 and else we get a mess when we compute mean and variance
//            hsv = cv2.cvtColor(bgrcap, cv2.COLOR_BGR2HSV);
//            var _tup_4 = cv2.split(hsv);
//            var h = _tup_4.Item1;
//            var s = _tup_4.Item2;
//            var v = _tup_4.Item3;
//            var h_mask = cv2.inRange(h, 0, 160);
//            h = cv2.bitwise_and(h, h, mask: h_mask);
//            hsv = cv2.merge((h, s, v)).astype(float);
//            // define two empty masks for the white-filter and the color-filter
//            color_mask = cv2.inRange(bgrcap, np.array(new List<int> {
//                1,
//                1,
//                1
//            }), np.array(new List<int> {
//                0,
//                0,
//                0
//            }));
//            white_mask = cv2.inRange(bgrcap, np.array(new List<int> {
//                1,
//                1,
//                1
//            }), np.array(new List<int> {
//                0,
//                0,
//                0
//            }));
//            cent = new List<object>();
//            find_squares(bgrcap, grid_N);
//            del_duplicates(cent);
//            // the medoid is the center which has the closest summed distances to the other centers
//            // It should be the center facelet of the cube
//            var m = medoid(cent);
//            var _tup_5 = facelets(cent, m);
//            var cf = _tup_5.Item1;
//            var ef = _tup_5.Item2;
//            // compute the alternate corner and edges facelet centers. These are the point reflections of an already
//            // known facelet center at the medoid center. Should some facelet center not be detected by itself it usually
//            // still is detected in this way.
//            var _tup_6 = mirr_facelet(cf, ef, m);
//            var acf = _tup_6.Item1;
//            var aef = _tup_6.Item2;
//            display_colorname(bgrcap, m);
//            foreach (var i in ef) {
//                display_colorname(bgrcap, i);
//            }
//            foreach (var i in cf) {
//                display_colorname(bgrcap, i);
//            }
//            foreach (var i in aef) {
//                display_colorname(bgrcap, i);
//            }
//            foreach (var i in acf) {
//                display_colorname(bgrcap, i);
//            }
//            // the results supplied by getcolors are used in client_gui2.py for the "Webcam import"
//            var _tup_7 = getcolors(cf, ef, acf, aef, m);
//            vision_params.face_hsv = _tup_7.Item1;
//            vision_params.face_col = _tup_7.Item2;
//            // drawgrid(bgrcap, grid_N)
//            // show the windows
//            cv2.imshow("color_filter mask", cv2.resize(color_mask, (width / 2, height / 2)));
//            cv2.imshow("white_filter mask", cv2.resize(white_mask, (width / 2, height / 2)));
//            cv2.imshow("black_filter mask", cv2.resize(black_mask, (width / 2, height / 2)));
//            cv2.imshow("Webcam - type \"x\" to quit.", bgrcap);
//            var k = cv2.waitKey(5) & 0xFF;
//            if (k == 120) {
//                // type x to exit
//                break;
//            }
//        }
//    }

//    static vision2() {
//        cv2.destroyAllWindows();
//    }
//}
