using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Gtm.Common;
using Gtm.Common.Collections;

namespace Gtm.WebService.Models
{
   /// <summary>
   /// Represents various measurements for selecting a size.
   /// </summary>
   [DataContract]
   public class SizingMeasurements : ICloneable<SizingMeasurements>
   {
      [DataMember(Name = "Sizes")]
      private IList<string> sizes = new NonEmptyStringList();

      [DataMember(Name = "Measurements")]
      private IList<TypeMeasurements> measurements = new NonNullList<TypeMeasurements>();

      /// <summary>
      /// Gets the list of all supported sizes.
      /// </summary>
      public IList<string> Sizes => sizes;

      /// <summary>
      /// Gets the list of all measurements.
      /// </summary>
      public IList<TypeMeasurements> Measurements => measurements;

      /// <inheritdoc />
      public SizingMeasurements Clone()
      {
         var cloned = new SizingMeasurements();

         foreach (var s in sizes)
            cloned.Sizes.Add(s);

         foreach (var m in measurements)
            cloned.Measurements.Add(m.Clone());

         return cloned;
      }

      /// <summary>
      /// Contains a list of measurements for a given measurement type (i.e. chest, waist).
      /// </summary>
      [DataContract]
      public class TypeMeasurements : ICloneable<TypeMeasurements>
      {
         [DataMember(Name = "MeasurementType")]
         private string measurementType;

         [DataMember(Name = "SizeMeasurements")]
         private IList<SizeMeasurement> sizeMeasurements = new NonNullList<SizeMeasurement>();

         /// <summary>
         /// Initializes a new instance of the <see cref="TypeMeasurements"/> class.
         /// </summary>
         /// <param name="measurementType">The measurement type to which all the <see cref="SizeMeasurements"/> belong.</param>
         /// <exception cref="ArgumentNullException">Thrown if <paramref name="measurementType"/> is <c>null</c>.</exception>
         /// <exception cref="ArgumentException">Thrown if <paramref name="measurementType"/> is empty or contains only white space.</exception>
         public TypeMeasurements(string measurementType)
         {
            ParameterVerifier.VerifyIsNotNullOrWhiteSpace(measurementType, nameof(measurementType));

            this.measurementType = measurementType;
         }

         /// <summary>
         /// Gets the type of measurement for the <see cref="SizeMeasurements"/>.
         /// </summary>
         public string MeasurementType => measurementType;

         /// <summary>
         /// Gets the list of measurements for each size for the <see cref="MeasurementType"/>.
         /// </summary>
         public IList<SizeMeasurement> SizeMeasurements => sizeMeasurements;

         /// <inheritdoc />
         public TypeMeasurements Clone()
         {
            var cloned = new TypeMeasurements(measurementType);

            foreach (var sm in sizeMeasurements)
               cloned.SizeMeasurements.Add(sm.Clone());

            return cloned;
         }
      }

      /// <summary>
      /// A measurement for a given size and type for use within a <see cref="SizingMeasurements"/>.
      /// </summary>
      [DataContract]
      public class SizeMeasurement : ICloneable<SizeMeasurement>
      {
         [DataMember(Name = "Size")]
         private string size;

         [DataMember(Name = "Measurement")]
         private string measurement;

         /// <summary>
         /// Initializes a new instance of the <see cref="SizeMeasurement"/> class.
         /// </summary>
         /// <param name="size">The size associated with the given <paramref name="measurement"/>.</param>
         /// <param name="measurement">The measurment text.</param>
         /// <exception cref="ArgumentNullException">
         /// Thrown if <paramref name="size"/> or <paramref name="measurement"/> is <c>null</c>.
         /// </exception>
         /// <exception cref="ArgumentException">
         /// Thrown if <paramref name="size"/> or <paramref name="measurement"/> is empty or contains only white space.
         /// </exception>
         public SizeMeasurement(string size, string measurement)
         {
            ParameterVerifier.VerifyIsNotNullOrWhiteSpace(size, nameof(size));
            ParameterVerifier.VerifyIsNotNullOrWhiteSpace(measurement, nameof(measurement));

            this.size = size;
            this.measurement = measurement;
         }

         /// <summary>
         /// Gets the size associated with the given <see cref="Measurement"/>.
         /// </summary>
         public string Size => size;

         /// <summary>
         /// Gets the measurement text.
         /// </summary>
         public string Measurement => measurement;

         /// <inheritdoc />
         public SizeMeasurement Clone()
         {
            return new SizeMeasurement(size, measurement);
         }
      }
   }
}