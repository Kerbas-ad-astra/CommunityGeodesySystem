#region license
//
//  CommunityGeodesyUtility.cs - various static utility methods used throughout the Community Geodesy System.
//
//  Author:
//       Kerbas_ad_astra <Kerbas-ad-astra@noreply.users.github.com>
//
//  Copyright (c) 2015 Kerbas_ad_astra
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using UnityEngine;

namespace CommunityGeodesySystem
{
	public static class CommunityGeodesyUtility
	{
		#region Public API Methods

		/// <summary>
		/// Converts latitude and longitude to a Cartesian 3D vector.
		/// </summary>
		/// <param name="lat">Clamped double in the -90 - 90 degree range</param>
		/// <param name="lon">Clamped double in the -180 - 180 degree range</param>
		/// <param name="rad">Radius of body, meters</param>
		/// <returns>Vector3 of indicated position (convention: prime meridian is +x, north pole is +z)</returns>
		public static Vector3d LatLon2Cart(double lat, double lon, double rad)
		{
			Vector3d vec = new Vector3d ();
			vec.x = rad * (Math.Cos (Deg2Rad (lon)) * Math.Cos (Deg2Rad (lat)));
			vec.y = rad * (Math.Sin (Deg2Rad (lon)) * Math.Cos (Deg2Rad (lat)));
			vec.z = rad * Math.Sin (Deg2Rad (lat));
			return vec;
		}
		
		/// <summary>
		/// Converts a Cartesian 3D vector to latitude and longitude.
		/// </summary>
		/// <param name="vec">Vector3d of indicated position (convention: prime meridian is +x, north pole is +z)</param>
		/// <returns>Pair of lat/lon coordinates (in degrees) of indicated position. </returns>
		public static double[] Cart2LatLon(Vector3d vec)
		{
			vec = Vector3d.Normalize (vec);
			double lat = Rad2Deg (Math.Asin (vec.z));
			double lon = Rad2Deg (Math.Atan2 (vec.x, vec.y));
			return new double[] { lat, lon };
		}

		/// <summary>
		/// Forward geodetic problem solver.  Given a staring point, if I head a given great-circle distance in a given initial direction, where do I land?
		/// </summary>
		/// <param name="lat">Clamped double in the -90 - 90 degree range</param>
		/// <param name="lon">Clamped double in the -180 - 180 degree range</param>
		/// <param name="head">Heading</param>
		/// <param name="dist">Distance traveled (meters)
		/// <param name="rad">Radius of body, meters</param>
		/// <returns>Trio of doubles: Pair of lat/lon coordinates of destination, and heading from there to current position. </returns>
		public static double[] ForwardGeoSolve(double lat1,double lon1, double head1, double dist, double rad)
		{
			double U1 = Deg2Rad (lat1);
			double alpha1 = Deg2Rad (head1);
			double sigma1 = Math.Atan2 (Math.Tan (U1), Math.Cos (alpha1));
			double sinAlpha = Math.Cos (U1) * Math.Sin (alpha1);
			double sigma = dist / rad;
			double lat2 = Rad2Deg (Math.Atan2 (Math.Sin (U1) * Math.Cos (sigma) + Math.Cos (U1) * Math.Sin (sigma) * Math.Cos (alpha1), Math.Sqrt (Math.Pow (sinAlpha, 2) + Math.Pow (Math.Sin (U1) * Math.Sin (sigma) - Math.Cos (U1) * Math.Cos (sigma) * Math.Cos (alpha1), 2))));
			double lambda = Rad2Deg (Math.Atan2 (Math.Sin (sigma) * Math.Sin (alpha1), Math.Cos (U1) * Math.Cos (sigma) + Math.Sin (U1) * Math.Sin (sigma) * Math.Cos (alpha1)));
			double lon2 = ClampDegrees180 (lon1 + lambda);
			double alpha2 = Rad2Deg (Math.Atan2 (sinAlpha, -1.0 * Math.Sin (U1) * Math.Sin (sigma) + Math.Cos (U1) * Math.Cos (sigma) * Math.Cos (alpha1)));
			return new double[] { lat2, lon2, alpha2 };
		}

		/// <summary>
		/// Inverse geodetic problem solver.  Given two points, how far apart are they, and what's the heading from one to the other?
		/// </summary>
		/// <param name="lat1">Clamped double in the -90 - 90 degree range</param>
		/// <param name="lon1">Clamped double in the -180 - 180 degree range</param>
		/// <param name="lat2">Clamped double in the -90 - 90 degree range</param>
		/// <param name="lon2">Clamped double in the -180 - 180 degree range</param>
		/// <param name="rad">Radius of body, meters</param>
		/// <returns>Trio of doubles: distance, heading from 1 to 2, heading from 2 to 1. </returns>
		public static double[] InverseGeoSolve(double lat1,double lon1, double lat2, double lon2, double rad)
		{
			double lambda = Deg2Rad (lon2 - lon1);
			double U1 = Deg2Rad (lat1);
			double U2 = Deg2Rad (lat2);
			double sinSigma = Math.Sqrt (Math.Pow (Math.Cos (U1) * Math.Sin (lambda), 2) + Math.Pow (Math.Cos (U1) * Math.Sin (U2) - Math.Sin (U1) * Math.Cos (U2) * Math.Cos (lambda), 2));
			double cosSigma = Math.Sin (U1) * Math.Sin (U2) + Math.Cos (U1) * Math.Cos (U2) * Math.Cos (lambda);
			double sigma = Math.Atan2 (sinSigma, cosSigma);
			double dist = sigma * rad;
			double head1 = Rad2Deg (Math.Atan2 (Math.Cos (U2) * Math.Sin (lambda), Math.Cos (U1) * Math.Sin (U2) - Math.Sin (U1) * Math.Cos (U2) * Math.Cos (lambda)));
			double head2 = Rad2Deg (Math.Atan2 (Math.Cos (U1) * Math.Sin (lambda), -1.0 * Math.Sin (U1) * Math.Cos (U2) + Math.Cos (U1) * Math.Sin (U2) * Math.Cos (lambda)));
			return new double[] { dist, head1, head2 };
		}

		/// <summary>
		/// Degrees to radians.
		/// </summary>
		/// <param name="deg">The angle in degrees</param>
		/// <returns>The angle in radians</returns>
		public static double Deg2Rad(double deg)
		{
			return deg*Math.PI/180.0;
		}

		/// <summary>
		/// Radians to degrees.
		/// </summary>
		/// <param name="rad">The angle in radians</param>
		/// <returns>The angle in degrees</returns>
		public static double Rad2Deg(double rad)
		{
			return rad * 180.0 / Math.PI;
		}

		// Some snippets from MechJeb (via SCANUtil)...
		public static double ClampDegrees360(double angle)
		{
			angle = angle % 360.0;
			if (angle < 0)
				return angle + 360.0;
			return angle;
		}
		//keeps angles in the range -180 to 180
		public static double ClampDegrees180(double angle)
		{
			angle = ClampDegrees360(angle);
			if (angle > 180)
				angle -= 360;
			return angle;
		}

		#endregion
	}
}