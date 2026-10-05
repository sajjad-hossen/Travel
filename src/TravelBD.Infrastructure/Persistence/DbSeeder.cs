using Microsoft.EntityFrameworkCore;
using TravelBD.Domain.Entities;
using TravelBD.Domain.Enums;

namespace TravelBD.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(TravelDbContext context)
    {
        if (await context.Locations.AnyAsync())
        {
            return; // Already seeded
        }

        // 1. LOCATIONS
        var dhaka = new Location
        {
            Name = "Dhaka",
            BanglaName = "ঢাকা",
            Slug = "dhaka",
            Type = LocationType.TransitHub,
            District = "Dhaka",
            Division = "Dhaka",
            Latitude = 23.8103,
            Longitude = 90.4125,
            IsMajorHub = true,
            IsTouristDestination = false,
            Description = "Capital city and the primary transit epicenter connecting every corner of Bangladesh.",
            HeroImageUrl = "https://images.unsplash.com/photo-1608958435020-e8a7109ba809?w=1200"
        };

        var chittagong = new Location
        {
            Name = "Chittagong",
            BanglaName = "চট্টগ্রাম",
            Slug = "chittagong",
            Type = LocationType.TransitHub,
            District = "Chattogram",
            Division = "Chattogram",
            Latitude = 22.3569,
            Longitude = 91.7832,
            IsMajorHub = true,
            IsTouristDestination = true,
            Description = "The commercial capital and primary gateway to the Southeastern Hill Tracts and Bay of Bengal beaches.",
            HeroImageUrl = "https://images.unsplash.com/photo-1596895111956-bf1cf0599ce5?w=1200"
        };

        var coxsBazar = new Location
        {
            Name = "Cox's Bazar",
            BanglaName = "কক্সবাজার",
            Slug = "coxs-bazar",
            Type = LocationType.TouristSpot,
            District = "Cox's Bazar",
            Division = "Chattogram",
            Latitude = 21.4272,
            Longitude = 92.0058,
            IsMajorHub = true,
            IsTouristDestination = true,
            Description = "World's longest unbroken natural sea beach stretching over 120 kilometers with breathtaking sunsets.",
            HeroImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1200"
        };

        var bandarban = new Location
        {
            Name = "Bandarban",
            BanglaName = "বান্দরবান",
            Slug = "bandarban",
            Type = LocationType.TouristSpot,
            District = "Bandarban",
            Division = "Chattogram",
            Latitude = 22.1953,
            Longitude = 92.2184,
            IsMajorHub = false,
            IsTouristDestination = true,
            Description = "The crown jewel of the Chittagong Hill Tracts featuring misty peaks, indigenous culture, and dramatic cascades.",
            HeroImageUrl = "https://images.unsplash.com/photo-1544620347-c4fd4a3d5957?w=1200"
        };

        var rangamati = new Location
        {
            Name = "Rangamati",
            BanglaName = "রাঙ্গামাটি",
            Slug = "rangamati",
            Type = LocationType.TouristSpot,
            District = "Rangamati",
            Division = "Chattogram",
            Latitude = 22.6533,
            Longitude = 92.1753,
            IsMajorHub = false,
            IsTouristDestination = true,
            Description = "Scenic lake district nestled around the emerald waters of Kaptai Lake with tribal handlooms and peaceful boat tours.",
            HeroImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=1200"
        };

        var sylhet = new Location
        {
            Name = "Sylhet",
            BanglaName = "সিলেট",
            Slug = "sylhet",
            Type = LocationType.District,
            District = "Sylhet",
            Division = "Sylhet",
            Latitude = 24.8949,
            Longitude = 91.8687,
            IsMajorHub = true,
            IsTouristDestination = true,
            Description = "Northeastern haven of rolling tea estates, Ratargul swamp forest, and spiritual shrines.",
            HeroImageUrl = "https://images.unsplash.com/photo-1578632767115-351597cf2477?w=1200"
        };

        var rajshahi = new Location
        {
            Name = "Rajshahi",
            BanglaName = "রাজশাহী",
            Slug = "rajshahi",
            Type = LocationType.District,
            District = "Rajshahi",
            Division = "Rajshahi",
            Latitude = 24.3745,
            Longitude = 88.6042,
            IsMajorHub = true,
            IsTouristDestination = false,
            Description = "The Silk City on the bank of the Padma River in northwest Bangladesh.",
            HeroImageUrl = "https://images.unsplash.com/photo-1518684079-3c830dcef090?w=1200"
        };

        var khulna = new Location
        {
            Name = "Khulna",
            BanglaName = "খুলনা",
            Slug = "khulna",
            Type = LocationType.District,
            District = "Khulna",
            Division = "Khulna",
            Latitude = 22.8456,
            Longitude = 89.5403,
            IsMajorHub = true,
            IsTouristDestination = false,
            Description = "Gateway to the mangrove expanse of the Sundarbans.",
            HeroImageUrl = "https://images.unsplash.com/photo-1513836279014-a89f7a76ae86?w=1200"
        };

        var barisal = new Location
        {
            Name = "Barisal",
            BanglaName = "বরিশাল",
            Slug = "barisal",
            Type = LocationType.District,
            District = "Barishal",
            Division = "Barishal",
            Latitude = 22.7010,
            Longitude = 90.3535,
            IsMajorHub = true,
            IsTouristDestination = false,
            Description = "The Venice of Bengal famous for floating guava markets and riverine networks.",
            HeroImageUrl = "https://images.unsplash.com/photo-1473448912268-2022ce9509d8?w=1200"
        };

        var thanchi = new Location
        {
            Name = "Thanchi",
            BanglaName = "থানচি",
            Slug = "thanchi",
            Type = LocationType.TouristSpot,
            District = "Bandarban",
            Division = "Chattogram",
            Latitude = 21.7865,
            Longitude = 92.4284,
            IsMajorHub = false,
            IsTouristDestination = true,
            Description = "Gateway to deep Bandarban treks, Sangu River boat rides, Amiakhum, and Nafakhum falls.",
            HeroImageUrl = "https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1200"
        };

        var sajek = new Location
        {
            Name = "Sajek Valley",
            BanglaName = "সাজেক ভ্যালি",
            Slug = "sajek-valley",
            Type = LocationType.TouristSpot,
            District = "Rangamati",
            Division = "Chattogram",
            Latitude = 23.3820,
            Longitude = 92.2938,
            IsMajorHub = false,
            IsTouristDestination = true,
            Description = "Queen of Hills above the clouds, located in Baghaichhari Upazila accessible via Dighinala army escort.",
            HeroImageUrl = "https://images.unsplash.com/photo-1486870591958-9b9d0d1dda99?w=1200"
        };

        var locations = new[] { dhaka, chittagong, coxsBazar, bandarban, rangamati, sylhet, rajshahi, khulna, barisal, thanchi, sajek };
        await context.Locations.AddRangeAsync(locations);
        await context.SaveChangesAsync();

        // 2. ROUTE SEGMENTS & TRANSPORT OPTIONS
        void AddBidirectionalLeg(Location a, Location b, double dist, int durMins, List<(TransportMode mode, RouteTier tier, string op, decimal minCost, decimal maxCost, int freq, string notes)> transports)
        {
            var segForward = new RouteSegment
            {
                OriginId = a.Id,
                DestinationId = b.Id,
                DistanceKm = dist,
                AvgDurationMinutes = durMins
            };
            var segBackward = new RouteSegment
            {
                OriginId = b.Id,
                DestinationId = a.Id,
                DistanceKm = dist,
                AvgDurationMinutes = durMins
            };

            foreach (var t in transports)
            {
                segForward.TransportOptions.Add(new TransportOption
                {
                    Mode = t.mode,
                    Tier = t.tier,
                    OperatorName = t.op,
                    MinCostBdt = t.minCost,
                    MaxCostBdt = t.maxCost,
                    FrequencyPerDay = t.freq,
                    ScheduleNotes = t.notes
                });

                segBackward.TransportOptions.Add(new TransportOption
                {
                    Mode = t.mode,
                    Tier = t.tier,
                    OperatorName = t.op,
                    MinCostBdt = t.minCost,
                    MaxCostBdt = t.maxCost,
                    FrequencyPerDay = t.freq,
                    ScheduleNotes = t.notes
                });
            }

            context.RouteSegments.AddRange(segForward, segBackward);
        }

        // Dhaka <-> Chittagong (Core Backbone)
        AddBidirectionalLeg(dhaka, chittagong, 245, 330, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Subarna / Sonar Bangla Express", 405, 850, 4, "Intercity high-speed train via Kamalapur to Chittagong station (5.5 hrs)."),
            (TransportMode.Bus, RouteTier.AcBus, "Green Line / Hanif / Desh Travels", 1000, 1600, 24, "Day & sleeper coaches via Dhaka-Chittagong 4-lane highway."),
            (TransportMode.Bus, RouteTier.NonAcBus, "Hanif / Shyamoli Paribahan", 680, 800, 40, "Budget frequent departures from Sayedabad & Gabtoli."),
            (TransportMode.Flight, RouteTier.EconomyAir, "Biman Bangladesh / US-Bangla / Air Astra", 3500, 6500, 8, "45-minute flight from Hazrat Shahjalal (DAC) to Shah Amanat (CGP).")
        });

        // Chittagong <-> Cox's Bazar
        AddBidirectionalLeg(chittagong, coxsBazar, 150, 150, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Cox's Bazar Express / Parjotok Express", 250, 500, 3, "Scenic new railway track over Karnaphuli and hill country (2.5 hrs)."),
            (TransportMode.Bus, RouteTier.AcBus, "S.Alam / Hanif / Saudia AC", 600, 900, 16, "Frequent AC coaches from Dampara and Cinema Palace."),
            (TransportMode.Bus, RouteTier.NonAcBus, "Purbani / Mars Paribahan", 380, 450, 30, "Fast local highway coaches.")
        });

        // Direct Dhaka <-> Cox's Bazar (Direct option)
        AddBidirectionalLeg(dhaka, coxsBazar, 395, 480, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Cox's Bazar Express (Direct Intercity)", 750, 1725, 2, "Iconic direct overnight & day intercity train arriving at the oyster-shaped station."),
            (TransportMode.Bus, RouteTier.SleeperBus, "Saintmartin Paribahan / Green Line Sleeper", 2000, 3200, 12, "Luxury multi-axle sleeper buses departing evening/night."),
            (TransportMode.Bus, RouteTier.AcBus, "Desh Travels / Shyamoli / Ena AC", 1400, 2000, 20, "Scania & Hyundai business class."),
            (TransportMode.Flight, RouteTier.EconomyAir, "US-Bangla / Novoair / Biman", 4500, 8500, 10, "Direct 55-minute flight from Dhaka to Cox's Bazar Airport (CXB).")
        });

        // Chittagong <-> Bandarban (Primary hill gate)
        AddBidirectionalLeg(chittagong, bandarban, 75, 135, new()
        {
            (TransportMode.Bus, RouteTier.NonAcBus, "Purbani Bus / Pubali Paribahan", 180, 250, 20, "Departs from Bahaddarhat Bus Terminal every 30 minutes (2 - 2.5 hrs)."),
            (TransportMode.ChanderGari, RouteTier.ReservedVehicle, "Private Mahendra / Chander Gari / HiAce", 3500, 5000, 5, "Door to door rental ideal for groups travelling with heavy baggage.")
        });

        // Direct Dhaka <-> Bandarban
        AddBidirectionalLeg(dhaka, bandarban, 320, 510, new()
        {
            (TransportMode.Bus, RouteTier.AcBus, "Saintmartin Travels / Shyamoli AC", 1400, 1800, 6, "Direct night coaches from Sayedabad & Arambagh to Bandarban town."),
            (TransportMode.Bus, RouteTier.NonAcBus, "S.Alam / Unique Service", 850, 950, 10, "Direct overnight buses reaching Bandarban early morning.")
        });

        // Chittagong <-> Rangamati
        AddBidirectionalLeg(chittagong, rangamati, 70, 120, new()
        {
            (TransportMode.Bus, RouteTier.NonAcBus, "Paharika Express / BRTC", 170, 230, 24, "From Muradpur or Oxygen bus stand to Rangamati terminal (2 hours)."),
            (TransportMode.Cng, RouteTier.ReservedVehicle, "Reserved Highway CNG / Car", 2000, 3000, 8, "Comfortable scenic mountain drive.")
        });

        // Direct Dhaka <-> Rangamati
        AddBidirectionalLeg(dhaka, rangamati, 310, 480, new()
        {
            (TransportMode.Bus, RouteTier.AcBus, "Hanif Enterprise / Shyamoli AC", 1200, 1600, 4, "Direct overnight services via Feni-Chittagong-Rangamati route."),
            (TransportMode.Bus, RouteTier.NonAcBus, "Unique / S.Alam", 800, 900, 8, "Daily scheduled departures from Dhaka.")
        });

        // Bandarban <-> Thanchi
        AddBidirectionalLeg(bandarban, thanchi, 82, 195, new()
        {
            (TransportMode.ChanderGari, RouteTier.LocalShared, "Chander Gari (Jeep / 4WD)", 250, 350, 6, "Local shared open jeep through dramatic mountain turns (3 - 3.5 hrs)."),
            (TransportMode.ChanderGari, RouteTier.ReservedVehicle, "Reserved 4x4 Chander Gari", 5500, 7500, 4, "Hired exclusively from Bandarban Jeep Station.")
        });

        // Rangamati <-> Sajek Valley
        AddBidirectionalLeg(rangamati, sajek, 125, 240, new()
        {
            (TransportMode.ChanderGari, RouteTier.ReservedVehicle, "Chander Gari (via Dighinala)", 7500, 10500, 10, "Traverses through scenic valleys and requires army convoy escort from Baghaichhari.")
        });

        // Remote Origins connecting to Dhaka
        AddBidirectionalLeg(sylhet, dhaka, 240, 330, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Parabat / Upaban Express", 380, 800, 4, "Comfortable rail trip through tea estates."),
            (TransportMode.Bus, RouteTier.AcBus, "Green Line / Shohagh", 800, 1300, 20, "Departures from Subhanighat.")
        });

        AddBidirectionalLeg(sylhet, chittagong, 370, 480, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Paharika / Udayan Express", 420, 890, 2, "Direct intercity train connecting Sylhet directly to Chittagong without passing Dhaka!"),
            (TransportMode.Bus, RouteTier.AcBus, "Ena / Shyamoli AC", 1100, 1500, 6, "Direct highway coach.")
        });

        AddBidirectionalLeg(rajshahi, dhaka, 250, 310, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Silk City / Padma Express", 400, 850, 4, "Fast broad-gauge rail across Bangabandhu Jamuna Bridge."),
            (TransportMode.Bus, RouteTier.AcBus, "National Travels / Desh Travels", 900, 1400, 18, "Direct AC coach.")
        });

        AddBidirectionalLeg(khulna, dhaka, 220, 230, new()
        {
            (TransportMode.Bus, RouteTier.AcBus, "Shohagh / Tungipara Express", 850, 1400, 16, "High-speed travel over the Padma Bridge in under 4 hours."),
            (TransportMode.Train, RouteTier.Snigdha, "Sundarban Express", 450, 950, 2, "Rail route via Padma Bridge rail link.")
        });

        AddBidirectionalLeg(barisal, dhaka, 180, 180, new()
        {
            (TransportMode.Bus, RouteTier.AcBus, "Sakura Paribahan / Green Line", 700, 1100, 24, "Express travel via Padma Bridge in ~3 hours."),
            (TransportMode.Launch, RouteTier.ReservedVehicle, "Sundarban / Suravi / Parabat Luxury Cabin", 1200, 3500, 6, "Iconic overnight riverine triple-decker giant launch with private VIP cabins.")
        });

        await context.SaveChangesAsync();

        // 3. ATTRACTIONS
        var attractions = new List<Attraction>
        {
            // Bandarban
            new Attraction
            {
                LocationId = bandarban.Id,
                Name = "Nilgiri Hill Resort Viewpoint",
                BanglaName = "নীলগিরি",
                Description = "Perched at 2,200 feet above sea level, managed by the Bangladesh Army, floating literally above cloud banks.",
                BestTimeToVisit = "September to February (Monsoon & Winter for sea of clouds)",
                EntryFeeBdt = 100,
                Category = "Cloud Peaks",
                ImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800"
            },
            new Attraction
            {
                LocationId = bandarban.Id,
                Name = "Nafakhum Waterfalls",
                BanglaName = "নাফাখুম জলপ্রপাত",
                Description = "The Bengal Niagara — a thunderous cascade located along the Remakri canal accessible via engine boat from Thanchi.",
                BestTimeToVisit = "October to January",
                EntryFeeBdt = 0,
                Category = "Waterfalls",
                ImageUrl = "https://images.unsplash.com/photo-1432405972618-c60b0225b8f9?w=800"
            },
            new Attraction
            {
                LocationId = bandarban.Id,
                Name = "Boga Lake",
                BanglaName = "বগালেক",
                Description = "A mystical natural crater lake at 1,500 feet enveloped in Marma folklore, halfway to Keokradong peak.",
                BestTimeToVisit = "Winter months (November to March)",
                EntryFeeBdt = 50,
                Category = "Lakes & Treks",
                ImageUrl = "https://images.unsplash.com/photo-1470071459604-3b5ec3a7fe05?w=800"
            },

            // Cox's Bazar
            new Attraction
            {
                LocationId = coxsBazar.Id,
                Name = "Inani & Himchari Marine Drive Beach",
                BanglaName = "ইনানী ও হিমছড়ি সমুদ্রসৈকত",
                Description = "A breathtaking drive along the longest marine drive in the world featuring coral stones, sea spray, and hill cliffs.",
                BestTimeToVisit = "Year round (Sunset is magical)",
                EntryFeeBdt = 30,
                Category = "Beaches",
                ImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=800"
            },
            new Attraction
            {
                LocationId = coxsBazar.Id,
                Name = "Saint Martin's Island Gateway (Teknaf / Cox)",
                BanglaName = "সেন্টমার্টিন দ্বীপ ট্রিপ",
                Description = "Bangladesh's only coral island boasting coconut groves, transparent azure water, and fresh crab barbecue.",
                BestTimeToVisit = "November to February (Ship season)",
                EntryFeeBdt = 0,
                Category = "Island",
                ImageUrl = "https://images.unsplash.com/photo-1519046904884-53103b34b206?w=800"
            },

            // Rangamati
            new Attraction
            {
                LocationId = rangamati.Id,
                Name = "Kaptai Lake Cruise & Hanging Bridge",
                BanglaName = "কাপ্তাই লেক ও ঝুলন্ত সেতু",
                Description = "The quintessential Rangamati experience — motorboat cruising across emerald blue hills and tribal villages.",
                BestTimeToVisit = "October to March",
                EntryFeeBdt = 50,
                Category = "Lakes",
                ImageUrl = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?w=800"
            },
            new Attraction
            {
                LocationId = rangamati.Id,
                Name = "Shuvolong Waterfalls",
                BanglaName = "শুভলং ঝর্ণা",
                Description = "Massive cliffside waterfall accessible exclusively by boat from Rangamati reserve bazaar.",
                BestTimeToVisit = "July to October (Monsoon flow is majestic)",
                EntryFeeBdt = 30,
                Category = "Waterfalls",
                ImageUrl = "https://images.unsplash.com/photo-1432405972618-c60b0225b8f9?w=800"
            },

            // Chittagong
            new Attraction
            {
                LocationId = chittagong.Id,
                Name = "Patenga Sea Beach & Karnaphuli River Estuary",
                BanglaName = "পতেঙ্গা সমুদ্র সৈকত",
                Description = "Beloved coast where the river meets the sea, famous for spiced crispy mud-crab fry and fresh falooda.",
                BestTimeToVisit = "Late afternoons for breeze and night lights",
                EntryFeeBdt = 0,
                Category = "Coast",
                ImageUrl = "https://images.unsplash.com/photo-1596895111956-bf1cf0599ce5?w=800"
            },
            new Attraction
            {
                LocationId = chittagong.Id,
                Name = "Naval Beach & Bangabandhu Tunnel",
                BanglaName = "বঙ্গবন্ধু টানেল ও নেভাল",
                Description = "South Asia's first underwater river tunnel connecting Chittagong city to Anwara beach.",
                BestTimeToVisit = "Evening drive",
                EntryFeeBdt = 150,
                Category = "Landmark",
                ImageUrl = "https://images.unsplash.com/photo-1477959858617-67f30bc75b82?w=800"
            }
        };

        await context.Attractions.AddRangeAsync(attractions);

        // 4. ACCOMMODATIONS
        var accommodations = new List<Accommodation>
        {
            // Bandarban
            new Accommodation
            {
                LocationId = bandarban.Id,
                Name = "Sairu Hill Resort",
                BudgetLevel = BudgetLevel.Resort,
                ApproxPriceRange = "৳12,000 - ৳22,000 / night",
                Address = "Baro Mile, Bandarban-Chimbuk Road",
                ContactPhone = "+8801777771234",
                Rating = 4.8,
                HighlightFeature = "Infinity pool with 360-degree panoramic mountain cloud view"
            },
            new Accommodation
            {
                LocationId = bandarban.Id,
                Name = "Hill Crown Hotel & Resort",
                BudgetLevel = BudgetLevel.MidRange,
                ApproxPriceRange = "৳3,500 - ৳6,000 / night",
                Address = "Near Circuit House, Bandarban Sadar",
                ContactPhone = "+8801819001122",
                Rating = 4.2,
                HighlightFeature = "Central location with family mountain view suites"
            },
            new Accommodation
            {
                LocationId = bandarban.Id,
                Name = "Boga Lake Tribal Community Homestay",
                BudgetLevel = BudgetLevel.Budget,
                ApproxPriceRange = "৳500 - ৳1,000 / night",
                Address = "Boga Lake Village, Ruma",
                ContactPhone = "+8801822334455",
                Rating = 4.5,
                HighlightFeature = "Authentic Marma/Bawm wooden stilt cottage experience & campfire"
            },

            // Cox's Bazar
            new Accommodation
            {
                LocationId = coxsBazar.Id,
                Name = "Sayeman Beach Resort",
                BudgetLevel = BudgetLevel.Luxury,
                ApproxPriceRange = "৳8,000 - ৳18,000 / night",
                Address = "Marine Drive, Kolatoli Beach",
                ContactPhone = "+8801755699944",
                Rating = 4.7,
                HighlightFeature = "Direct oceanfront balcony & infinity pool facing Bay of Bengal"
            },
            new Accommodation
            {
                LocationId = coxsBazar.Id,
                Name = "Hotel Sea Crown",
                BudgetLevel = BudgetLevel.MidRange,
                ApproxPriceRange = "৳3,000 - ৳5,500 / night",
                Address = "Kolatoli Beach Front",
                ContactPhone = "+8801819876543",
                Rating = 4.3,
                HighlightFeature = "Private beach sitting lounge and open grill restaurant"
            },

            // Rangamati
            new Accommodation
            {
                LocationId = rangamati.Id,
                Name = "Aronnook Holiday Resort",
                BudgetLevel = BudgetLevel.Resort,
                ApproxPriceRange = "৳5,000 - ৳10,000 / night",
                Address = "Kaptai Lake Shore, Rangamati Sadar",
                ContactPhone = "+8801769300000",
                Rating = 4.6,
                HighlightFeature = "Overlooking Kaptai Lake with army leisure security & pedal boats"
            },
            new Accommodation
            {
                LocationId = rangamati.Id,
                Name = "Hotel Lake View",
                BudgetLevel = BudgetLevel.Budget,
                ApproxPriceRange = "৳1,500 - ৳2,800 / night",
                Address = "Reserve Bazaar, Rangamati",
                ContactPhone = "+8801815112233",
                Rating = 4.0,
                HighlightFeature = "Walkable to boat terminal for early morning Lake cruise"
            },

            // Sajek
            new Accommodation
            {
                LocationId = sajek.Id,
                Name = "Meghpunji Resort",
                BudgetLevel = BudgetLevel.EcoCottage,
                ApproxPriceRange = "৳4,500 - ৳8,000 / night",
                Address = "Ruilui Para, Sajek Valley",
                ContactPhone = "+8801883000000",
                Rating = 4.9,
                HighlightFeature = "Glass windows directly facing rolling white cloud valleys"
            }
        };

        await context.Accommodations.AddRangeAsync(accommodations);

        // 5. ADVISORIES & LOCAL TIPS
        var advisories = new List<DestinationAdvisory>
        {
            // Bandarban
            new DestinationAdvisory
            {
                LocationId = bandarban.Id,
                Category = "Mandatory Hill Tracts Permits",
                Title = "NID / Passport Copies Required for Army Checkposts",
                Content = "Carry at least 5-10 photocopies of your National ID card / Passport and 2 passport-size photographs. Every tourist must register at Army and Police checkposts at Ruma, Thanchi, and Chimbuk before proceeding.",
                IsMandatory = true
            },
            new DestinationAdvisory
            {
                LocationId = bandarban.Id,
                Category = "Guide Requirement",
                Title = "Registered Local Guide for Remakri / Nafakhum",
                Content = "Venturing beyond Thanchi into Remakri or Nafakhum waterfalls strictly requires hiring a certified guide from the Thanchi Guide Association (approx ৳1,500/day + food). Life jackets are compulsory when riding engine boats through Tindu boulders.",
                IsMandatory = true
            },
            new DestinationAdvisory
            {
                LocationId = bandarban.Id,
                Category = "Food Speciality",
                Title = "Tribal Cuisine: Bamboo Chicken & Mundi",
                Content = "Try indigenous Marma and Tripura delicacies. 'Bamboo Chicken' (chicken slow-cooked inside green bamboo culms over charcoal) and 'Mundi' (spicy homemade rice noodles served in hot aromatic broth) are available near Thanchi and Chimbuk hill shops.",
                IsMandatory = false
            },

            // Cox's Bazar
            new DestinationAdvisory
            {
                LocationId = coxsBazar.Id,
                Category = "Food Speciality",
                Title = "Fresh Seafood at Jhaubon & Poushee",
                Content = "Don't miss Ruponchanda fry, Koral fish bhuna, Loitta fry, and dried fish (Shutki) bharta at local landmark eateries like Poushee, Jhaubon, and Taranga near Kolatoli.",
                IsMandatory = false
            },
            new DestinationAdvisory
            {
                LocationId = coxsBazar.Id,
                Category = "Safety",
                Title = "Red Flag High Tide Warning",
                Content = "Observe beach safety lifeguard flags. Swimming during high tide or near rip currents (especially at Laboni and Sugandha points) is prohibited when red flags are raised.",
                IsMandatory = true
            },

            // Rangamati
            new DestinationAdvisory
            {
                LocationId = rangamati.Id,
                Category = "Local Transport",
                Title = "Renting Engine Boats at Reserve Bazaar",
                Content = "For visiting Shuvolong Waterfalls or Peda Ting Ting island restaurant, negotiate boat rates at Reserve Bazaar Ghat (typical day rental ranges ৳2,000 - ৳3,500 depending on boat capacity).",
                IsMandatory = false
            },
            new DestinationAdvisory
            {
                LocationId = rangamati.Id,
                Category = "Food Speciality",
                Title = "Peda Ting Ting & Kehang Bamboo Fish",
                Content = "Take a boat to lake restaurants like Peda Ting Ting or Tuk Tuk Eco Village to taste fresh Kaptai Lake carp roasted in bamboo tubes and native sticky rice with wild herb chutneys.",
                IsMandatory = false
            },

            // Sajek
            new DestinationAdvisory
            {
                LocationId = sajek.Id,
                Category = "Mandatory Escort",
                Title = "Army Convoy Timings from Dighinala",
                Content = "Vehicles cannot travel independently into Sajek Valley. You must join the Bangladesh Army Convoy from Dighinala checkpoint, which departs twice daily: Morning Convoy at 10:00 AM and Afternoon Convoy at 3:00 PM.",
                IsMandatory = true
            }
        };

        await context.DestinationAdvisories.AddRangeAsync(advisories);
        await context.SaveChangesAsync();
    }
}
